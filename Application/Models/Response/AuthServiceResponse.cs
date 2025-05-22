namespace Application.Models.Response;

public record AuthServiceResponse
{
    public required string RefreshToken { get; set; }
    public required string AccessToken { get; set; }
    public required Guid UserId { get; set; }
}
