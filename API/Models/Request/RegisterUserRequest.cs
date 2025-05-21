namespace API.Models.Request;

public sealed record RegisterUserRequest
{
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
};