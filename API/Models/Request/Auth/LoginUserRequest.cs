namespace API.Models.Request.Auth;

public sealed record LoginUserRequest
{
    public string Email { get; init; } = null!;
    public  string Password { get; init; } = null!;
}
