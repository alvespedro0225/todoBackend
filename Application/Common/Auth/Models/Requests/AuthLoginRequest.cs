namespace Application.Common.Auth;

public sealed record AuthLoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}