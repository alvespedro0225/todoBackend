namespace Application.Services.Auth;

public record AuthResponse
{
    public required string RefreshToken { get; set; }
    public required string AccessToken { get; set; }
    public required Guid UserId { get; set; }
}
