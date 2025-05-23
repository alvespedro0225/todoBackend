using Application.Common.Auth;
using Application.Common.Exceptions;
using Application.Common.Repositories;
using Application.Services.Common;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Auth.Commands;

public sealed class AuthCommandService(
    IJwtTokenGenerator tokenGenerator,
    IDateTimeProvider dateTime,
    IConfiguration configuration,
    IUserRepository userRepository)
    : IAuthCommandService
{
    private readonly int _refreshExpirationTime = configuration.GetValue<int>("Jwt:RefreshExpirationInMinutes");

    
    public AuthResponse Login(string email, string password)
    {
        var user = userRepository.GetUser(email);
        
        if (user is null ||
            password != user.Password)
            throw new UnauthorizedException("Error during login", "Password and email don't match");
        
        user.RefreshToken = tokenGenerator.GenerateRefreshToken();
        user.RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(_refreshExpirationTime);
        
        var response = new AuthResponse
        {
            AccessToken = tokenGenerator.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken,
            UserId = user.Id
        };
        
        return response;
    }

    public AuthResponse Register(string name, string email, string password)
    {
        if (userRepository.GetUser(email) is not null)
            throw new UnprocessableEntityException("Email already registered", "Please use another email");
        
        var user = new User
        {
            Name = name,
            Email = email,
            Password = password,
            RefreshToken = tokenGenerator.GenerateRefreshToken(),
            RefreshTokenExpiration = dateTime.UtcNow.AddMinutes(_refreshExpirationTime)
        };
        
        userRepository.AddUser(user);
        
        var response = new AuthResponse
        {
            AccessToken = tokenGenerator.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken,
            UserId = user.Id
        };
        return response;
    }

    public string RefreshAccessToken(Guid userId, string providedToken)
    {
        var user = userRepository.GetUser(userId);

        if (user is null)
            throw new NotFoundException("User not found", "Make sure this user is registered");
                
        if(user.RefreshToken != providedToken)
            throw new UnauthorizedException("Invalid refresh token", "Make sure you have the right token");
                        
        if (user.RefreshTokenExpiration <= dateTime.UtcNow)
            throw new UnauthorizedException("Expired refresh token", "Login again to get a new token");

        return tokenGenerator.GenerateAccessToken(user);
    }
}