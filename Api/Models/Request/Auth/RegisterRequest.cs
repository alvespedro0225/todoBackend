namespace Api.Models.Request.Auth;

public sealed record RegisterRequest
{
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
};