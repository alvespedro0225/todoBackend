using Application.Common.Interfaces.Auth;
using Application.Common.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Auth;

public sealed class AuthService(
    IJwtTokenGenerator tokenGenerator,
    IDateTimeProvider dateTime,
    IConfiguration configuration)
    : IAuthService
{
    private readonly int _refreshExpirationTime = configuration.GetValue<int>("Jwt:RefreshExpirationInMinutes");

    private readonly List<User> _users = [];
    
    public AuthResponse? Login(string email, string password)
    {
        var user = _users.FirstOrDefault(listUser => listUser.Email == email);
        
        if (user is null)
            return null;
        
        user.RefreshToken = tokenGenerator.GenerateRefreshToken();
        user.RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(_refreshExpirationTime);
        
        var response = new AuthResponse
        {
            AccessToken = tokenGenerator.GenerateAccessToken(user.Id, user.Name, user.Email),
            RefreshToken = user.RefreshToken,
            UserId = user.Id
        };
        
        return user.Password == password ? response : null;
    }

    public AuthResponse? Register(string name, string email, string password)
    {
        if (_users.FirstOrDefault(user => user.Email == email) is not null)
            return null;
        
        var id = Guid.NewGuid();
        var user = new User
        {
            Name = name,
            Email = email,
            Id = id,
            Password = password,
            RefreshToken = tokenGenerator.GenerateRefreshToken(),
            RefreshTokenExpiration = dateTime.UtcNow.AddMinutes(_refreshExpirationTime)
        };
        
        _users.Add(user);
        
        var response = new AuthResponse
        {
            AccessToken = tokenGenerator.GenerateAccessToken(user.Id, user.Name, user.Email),
            RefreshToken = user.RefreshToken,
            UserId = user.Id
        };
        return response;
    }

    public string? RefreshAccessToken(Guid id, string providedToken)
    {
        var user = _users.FirstOrDefault(listUser => listUser.Id == id);

        if (user is null 
            || user.RefreshToken != providedToken 
            || user.RefreshTokenExpiration <= dateTime.UtcNow)
            return null;

        return tokenGenerator.GenerateAccessToken(user.Id, user.Name, user.Email);
    }
}