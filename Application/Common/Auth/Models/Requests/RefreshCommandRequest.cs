namespace Application.Common.Auth.Models.Requests;

public sealed record RefreshCommandRequest
{
    public required Guid UserId { get; init; }
    public required string RefreshToken { get; init; }
}