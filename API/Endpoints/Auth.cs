using API.Models.Request;
using API.Models.Request.Auth;
using API.Validators.Auth;
using Application.Services;
using Application.Services.Auth.Commands;
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

    public static IResult Register(RegisterUserRequest registerRequest, IAuthCommandService authCommandService)
    {
        ValidateAuth(new RegisterUserRequestValidator(), registerRequest);
        
        var registration = authCommandService.Register(
            registerRequest.Name,
            registerRequest.Email,
            registerRequest.Password);

        return Results.Ok(registration);
    }

    public static IResult Login(LoginUserRequest loginRequest, IAuthCommandService authCommandService)
    {
        ValidateAuth(new LoginUserRequestValidator(), loginRequest);

        var token = authCommandService.Login(loginRequest.Email, loginRequest.Password);

        return Results.Ok(token);
    }

    public static IResult Refresh(RefreshTokenRequest refreshTokenRequest, IAuthCommandService authCommandService)
    {
        ValidateAuth(
            new RefreshTokenRequestValidator(),
            refreshTokenRequest);
        
        var token = authCommandService.RefreshAccessToken(refreshTokenRequest.Id, refreshTokenRequest.RefreshToken); 
        return Results.Ok(token);
    }
    
    private static void ValidateAuth<T>(AbstractValidator<T> validator, T todo)
    {
        validator.ValidateAndThrow(todo);
    }
}