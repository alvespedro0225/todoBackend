using API.Models.Request;
using API.Validators;
using Application.Services.Auth;

namespace API.Endpoints;

public static class Auth
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("auth");
        group.MapPost("login", Login);
        group.MapPost("register", Register);
    }

    public static IResult Register(RegisterUserRequest request, IAuthService authService)
    {
        var validator = new RegisterUserRequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
            Results.BadRequest(validationResult.Errors);
        
        var registration = authService.Register(request.Name, request.Email, request.Password);

        return registration is not null 
            ? Results.Ok(registration) 
            : Results.UnprocessableEntity("Email already registered");
    }

    public static IResult Login(LoginUserRequest request, IAuthService authService)
    {
        var validator = new LoginUserRequestValidator();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var token = authService.Login(request.Email, request.Password);

        return token is not null 
            ? Results.Ok(token) 
            : Results.BadRequest("Password and email don't match");
    }
}