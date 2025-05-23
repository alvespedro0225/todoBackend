namespace API.Models.Request.Auth;

public sealed record RefreshRequest
{
    public string RefreshToken { get; init; } = null!;
    public Guid Id { get; init; }
}