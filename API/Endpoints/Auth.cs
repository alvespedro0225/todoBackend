using System.Security.Claims;

using Api.Models.Request.Auth;
using Api.Validators.Auth;

using Application.Common.Auth;
using Application.Common.Auth.Models.Requests;
using Application.Services.Auth.Commands;
using Application.Services.Auth.Queries;

using Domain.Constants;
using Domain.Exceptions;

using FluentValidation;

namespace Api.Endpoints;

public static class Auth
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("auth");
        var authGroup = app.MapGroup("auth").RequireAuthorization();
        group.MapPost("login", LoginUser);
        group.MapPost("register", RegisterUser);
        group.MapPost("refresh", RefreshToken);
        authGroup.MapDelete("delete/{deleteUserId:guid}", DeleteUser);
    }


    public static async Task<IResult> RegisterUser(
        RegisterRequest registerRequest,
        IAuthCommandService authCommandService)
    {
         ValidateAuth(new RegisterRequestValidator(), registerRequest);

        var registration = await authCommandService.RegisterUser(new RegisterCommandRequest
        {
            Name = registerRequest.Name,
            Email = registerRequest.Email,
            Password = registerRequest.Password
        });

        return TypedResults.Ok(registration);
    }

    public static async Task<IResult> LoginUser(
        LoginRequest loginRequest,
        IAuthQueryService authQueryService)
    {
        ValidateAuth(new LoginRequestValidator(), loginRequest);

        var token = await authQueryService.Login(new LoginCommandRequest
        {
            Email = loginRequest.Email,
            Password = loginRequest.Password
        });

        return TypedResults.Ok(token);
    }

    public static async Task<IResult> RefreshToken(
        RefreshRequest refreshRequest,
        IAuthQueryService authQueryService)
    {
        ValidateAuth(
            new RefreshRequestValidator(),
            refreshRequest);

        var token = await authQueryService.RefreshAccessToken(new RefreshCommandRequest
        {
            UserId = refreshRequest.Id,
            RefreshToken = refreshRequest.RefreshToken
        });
        
        return TypedResults.Ok(token);
    }
    
    public static async Task<IResult> DeleteUser(
        HttpContext context,
        Guid deleteUserId,
        IAuthCommandService authCommandService
        )
    {
        var userId = GetUserId(context);
        
        if (userId != deleteUserId)
            throw new ForbiddenException(
                "Cannot delete another user.",
                "You do not have permission to delete this user.");

        await authCommandService.DeleteUser(userId);
        return TypedResults.NoContent();
    }
    private static void ValidateAuth<T>(AbstractValidator<T> validator, T validatedItem)
    {
        validator.ValidateAndThrow(validatedItem);
    }
    private static Guid GetUserId(HttpContext context)
    {
        var user = context.User;
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            throw new UnauthorizedException(
                DefaultErrorMessages.UnauthorizedMissingClaimError,
                DefaultErrorMessages.UnauthorizedMissingClaimMessage);

        var stringUserId = userIdClaim.Value;

        if (!Guid.TryParse(stringUserId, out var userId))
            throw new UnauthorizedException(
                DefaultErrorMessages.UnauthorizedInvalidClaimError,
                DefaultErrorMessages.UnauthorizedInvalidClaimMessage);

        return userId;
    }
}