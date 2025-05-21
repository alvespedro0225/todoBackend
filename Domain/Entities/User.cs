namespace Domain.Entities;

public class User
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public Guid Id { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
}