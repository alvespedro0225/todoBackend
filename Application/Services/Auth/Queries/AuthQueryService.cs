using Application.Common.Auth;
using Application.Common.Exceptions;
using Application.Common.Repositories;
using Application.Services.Common;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Auth.Queries;

public sealed class AuthQueryService(
    IUserRepository userRepository,
    IJwtTokenGenerator tokenGenerator,
    IConfiguration configuration,
    IDateTimeProvider dateTime) : IAuthQueryService
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