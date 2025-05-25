using Application.Common.Auth;
using Application.Common.Auth.Models.Requests;
using Application.Common.Auth.Models.Responses;
using Application.Common.Repositories;
using Application.Services.Common;
using Domain.Entities;
using Domain.Exceptions;

using Microsoft.AspNetCore.Identity;
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
    
    public async Task<AuthResponse> RegisterUser(RegisterCommandRequest registerCommandRequest)
    {
        if (await userRepository.UserExists(registerCommandRequest.Email))
            throw new UnprocessableEntityException("Email already registered", "Please use another email");

        var passwordHasher = new PasswordHasher<User>();
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = registerCommandRequest.Name,
            Email = registerCommandRequest.Email,
            PasswordHash = registerCommandRequest.Password,
            RefreshToken = tokenGenerator.GenerateRefreshToken(),
            RefreshTokenExpiration = dateTime.UtcNow.AddMinutes(_refreshExpirationTime)
        };
        
        user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);
        await userRepository.AddUser(user);
        
        var response = new AuthResponse
        {
            AccessToken = tokenGenerator.GenerateAccessToken(user),
            RefreshToken = user.RefreshToken,
            UserId = user.Id
        };
        return response;
    }

    public async Task DeleteUser(Guid userId)
    {
        await userRepository.DeleteUser(userId);
    }
}