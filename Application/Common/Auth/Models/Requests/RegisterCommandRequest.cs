namespace Application.Common.Auth.Models.Requests;

public sealed record RegisterCommandRequest
{
    public required string Name { get; set; }
    public required string Email { get; init; }
    public required string Password { get; set; }
}