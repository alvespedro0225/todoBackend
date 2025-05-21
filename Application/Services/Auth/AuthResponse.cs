namespace Application.Services.Auth;

public record AuthResponse
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Token { get; set; }

    public Guid Id { get; set; }
}
