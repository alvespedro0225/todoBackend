using API.Models.Request;
using API.Models.Request.Auth;
using API.Validators.Auth;
using Application.Services;
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

    public static IResult Register(RegisterUserRequest registerRequest, IAuthService authService)
    {
        ValidateAuth(new RegisterUserRequestValidator(), registerRequest);
        
        var registration = authService.Register(
            registerRequest.Name,
            registerRequest.Email,
            registerRequest.Password);

        return Results.Ok(registration);
    }

    public static IResult Login(LoginUserRequest loginRequest, IAuthService authService)
    {
        ValidateAuth(new LoginUserRequestValidator(), loginRequest);

        var token = authService.Login(loginRequest.Email, loginRequest.Password);

        return Results.Ok(token);
    }

    public static IResult Refresh(RefreshTokenRequest refreshTokenRequest, IAuthService authService)
    {
        ValidateAuth(
            new RefreshTokenRequestValidator(),
            refreshTokenRequest);
        
        var token = authService.RefreshAccessToken(refreshTokenRequest.Id, refreshTokenRequest.RefreshToken); 
        return Results.Ok(token);
    }
    
    private static void ValidateAuth<T>(AbstractValidator<T> validator, T todo)
    {
        validator.ValidateAndThrow(todo);
    }
}