namespace Application.Common.Auth;

public sealed record LoginCommandRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}