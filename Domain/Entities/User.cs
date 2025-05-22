namespace Domain.Entities;

public sealed record User
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RefreshToken { get; set; } = null!;
    public DateTime RefreshTokenExpiration { get; set; }
    public Guid Owner { get; set; } 
}