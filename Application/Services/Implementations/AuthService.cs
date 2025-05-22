using System.Security.Authentication;
using Application.Common.Auth;
using Application.Common.Repositories;
using Application.Models.Response;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Implementations;

public sealed class AuthService(
    IJwtTokenGenerator tokenGenerator,
    IDateTimeProvider dateTime,
    IConfiguration configuration,
    IUserRepository userRepository)
    : IAuthService
{
    private readonly int _refreshExpirationTime = configuration.GetValue<int>("Jwt:RefreshExpirationInMinutes");

    
    public AuthServiceResponse Login(string email, string password)
    {
        var user = userRepository.GetUserFromEmail(email);
        
        if (user is null ||
            password != user.Password)
            throw new InvalidCredentialException("Password and email don't match");
        
        user.RefreshToken = tokenGenerator.GenerateRefreshToken();
        user.RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(_refreshExpirationTime);
        
        var response = new AuthServiceResponse
        {
            AccessToken = tokenGenerator.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken,
            UserId = user.Id
        };
        
        return response;
    }

    public AuthServiceResponse Register(string name, string email, string password)
    {
        if (userRepository.GetUserFromEmail(email) is not null)
            throw new Exception("Email already registered");
        
        var user = new User
        {
            Name = name,
            Email = email,
            Password = password,
            RefreshToken = tokenGenerator.GenerateRefreshToken(),
            RefreshTokenExpiration = dateTime.UtcNow.AddMinutes(_refreshExpirationTime)
        };
        
        userRepository.AddUser(user);
        
        var response = new AuthServiceResponse
        {
            AccessToken = tokenGenerator.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken,
            UserId = user.Id
        };
        return response;
    }

    public string RefreshAccessToken(Guid id, string providedToken)
    {
        var user = userRepository.GetUserFromId(id);

        if (user is null)
            throw new Exception("User not found");
                
        if(user.RefreshToken != providedToken)
            throw new Exception("Invalid refresh token");
                        
        if (user.RefreshTokenExpiration <= dateTime.UtcNow)
            throw new Exception("Expired refresh token");

        return tokenGenerator.GenerateAccessToken(user);
    }
}