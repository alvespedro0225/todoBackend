namespace Application.Common.Auth.Models.Requests;

public sealed record AuthRegisterRequest
{
    public required string Name { get; set; }
    public required string Email { get; init; }
    public required string Password { get; set; }
}