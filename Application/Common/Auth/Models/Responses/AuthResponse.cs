namespace Application.Common.Auth.Models.Responses;

public sealed record AuthResponse
{
    public required string RefreshToken { get; set; }
    public required string AccessToken { get; set; }
    public required Guid UserId { get; set; }
}
