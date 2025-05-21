namespace API.Models.Request;

public sealed record RefreshUserRequest
{
    public required string RefreshToken { get; init; }
    public Guid Id { get; init; }
}