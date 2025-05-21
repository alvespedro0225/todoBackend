namespace API.Models.Request;

public sealed record LoginUserRequest
{
    public string Email { get; init; } = null!;
    public  string Password { get; init; } = null!;
}
