namespace Api1.Models.Response.Auth;

public sealed record RefreshResponse
{
    public required string AccessToken { get; set; }
}