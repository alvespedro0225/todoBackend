using Application.Common.Auth;
using Application.Common.Auth.Models.Requests;
using Application.Common.Auth.Models.Responses;
using Application.Common.Repositories;
using Application.Services.Common;
using Domain.Entities;
using Domain.Exceptions;
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
    
    public AuthResponse Register(RegisterCommandRequest registerCommandRequest)
    {
        if (userRepository.GetUser(registerCommandRequest.Email) is not null)
            throw new UnprocessableEntityException("Email already registered", "Please use another email");
        
        var user = new User
        {
            Name = registerCommandRequest.Name,
            Email = registerCommandRequest.Email,
            Password = registerCommandRequest.Password,
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
}