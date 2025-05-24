using Application.Common.Auth;
using Application.Common.Auth.Models.Requests;
using Application.Common.Auth.Models.Responses;
using Application.Common.Repositories;
using Application.Services.Common;

using Domain.Entities;
using Domain.Exceptions;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Auth.Queries;

public sealed class AuthQueryService(
    IUserRepository userRepository,
    IJwtTokenGenerator tokenGenerator,
    IConfiguration configuration,
    IDateTimeProvider dateTime) : IAuthQueryService
{
    private readonly int _refreshExpirationTime = configuration.GetValue<int>("Jwt:RefreshExpirationInMinutes");
    
    public async Task<AuthResponse> Login(LoginCommandRequest loginCommandRequest)
    {
        var user = await userRepository.GetUser(loginCommandRequest.Email);
        var passwordHasher = new PasswordHasher<User>();
        var passwordVerificationResult = passwordHasher
            .VerifyHashedPassword(
                user,
                user.PasswordHash,
                loginCommandRequest.Password);
        
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
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
    
    public async Task<string> RefreshAccessToken(RefreshCommandRequest refreshCommandRequest)
    {
        var user = await userRepository.GetUser(refreshCommandRequest.UserId);

        if (user is null)
            throw new NotFoundException("User not found", "Make sure this user is registered");
                
        if(user.RefreshToken != refreshCommandRequest.RefreshToken)
            throw new UnauthorizedException("Invalid refresh token", "Make sure you have the right token");
                        
        if (user.RefreshTokenExpiration <= dateTime.UtcNow)
            throw new UnauthorizedException("Expired refresh token", "Login again to get a new token");

        return tokenGenerator.GenerateAccessToken(user);
    }

    public async Task<User> GetUser(Guid userId) => await userRepository.GetUser(userId);
}