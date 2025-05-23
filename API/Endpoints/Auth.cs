using API.Models.Request.Auth;
using API.Validators.Auth;
using Application.Common.Auth;
using Application.Common.Auth.Models.Requests;
using Application.Services.Auth.Commands;
using Application.Services.Auth.Queries;
using FluentValidation;

namespace API.Endpoints;

public static class Auth
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("auth");
        group.MapPost("login", Login);
        group.MapPost("register", Register);
        group.MapPost("refresh", Refresh);
    }

    public static IResult Register(
        RegisterRequest registerRequest,
        IAuthCommandService authCommandService)
    {
        ValidateAuth(new RegisterRequestValidator(), registerRequest);

        var registration = authCommandService.Register(new RegisterCommandRequest
        {
            Name = registerRequest.Name,
            Email = registerRequest.Email,
            Password = registerRequest.Password
        });

        return Results.Ok(registration);
    }

    public static IResult Login(
        LoginRequest loginRequest,
        IAuthQueryService authQueryService)
    {
        ValidateAuth(new LoginRequestValidator(), loginRequest);

        var token = authQueryService.Login(new LoginCommandRequest
        {
            Email = loginRequest.Email,
            Password = loginRequest.Password
        });

        return Results.Ok(token);
    }

    public static IResult Refresh(
        RefreshRequest refreshRequest,
        IAuthQueryService authQueryService)
    {
        ValidateAuth(
            new RefreshRequestValidator(),
            refreshRequest);

        var token = authQueryService.RefreshAccessToken(new RefreshCommandRequest
        {
            UserId = refreshRequest.Id,
            RefreshToken = refreshRequest.RefreshToken
        });
        
        return Results.Ok(token);
    }
    
    private static void ValidateAuth<T>(AbstractValidator<T> validator, T todo)
    {
        validator.ValidateAndThrow(todo);
    }
}