using API.Models.Request;
using API.Validators;
using API.Validators.Auth;
using Application.Services;
using FluentValidation;
using FluentValidation.Results;

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
        var validationResult = ValidateAuth(
            new RegisterUserRequestValidator(),
            registerRequest,
            out var errors);

        if (!validationResult)
            return Results.BadRequest(errors);
        
        var registration = authService.Register(registerRequest.Name, registerRequest.Email, registerRequest.Password);

        return Results.Ok(registration);
    }

    public static IResult Login(LoginUserRequest loginRequest, IAuthService authService)
    {
        var validationResult = ValidateAuth(
            new LoginUserRequestValidator(),
            loginRequest,
            out var errors);
        
        if (!validationResult)
            return Results.BadRequest(validationResult);

        var token = authService.Login(loginRequest.Email, loginRequest.Password);

        return Results.Ok(token);
    }

    public static IResult Refresh(RefreshTokenRequest refreshTokenRequest, IAuthService authService)
    {
        var validationResult = ValidateAuth(
            new RefreshTokenRequestValidator(),
            refreshTokenRequest,
            out var errors);
        
        if (!validationResult)
            return Results.BadRequest(validationResult);
        
        var token = authService.RefreshAccessToken(refreshTokenRequest.Id, refreshTokenRequest.RefreshToken); 
        return Results.Ok(token);
    }
    
    private static bool ValidateAuth<T>(AbstractValidator<T> validator, T todo, out List<ValidationFailure> errors)
    {
        var validationResult = validator.Validate(todo);
        errors = validationResult.Errors;
        return validationResult.IsValid;
    }
}