using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public sealed class User : Entity
{
    [MaxLength(40)]
    public required string Name { get; set; }
    [MaxLength(40)]
    public required string Email { get; set; }
    [MaxLength(40)]
    public required string PasswordHash { get; set; }
    [MaxLength(100)]
    public required string RefreshToken { get; set; }
    public required DateTime RefreshTokenExpiration { get; set; }
}