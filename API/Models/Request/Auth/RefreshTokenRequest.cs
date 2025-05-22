namespace API.Models.Request;

public sealed record RefreshTokenRequest
{
    public string RefreshToken { get; init; } = null!;
    public Guid Id { get; init; }
}