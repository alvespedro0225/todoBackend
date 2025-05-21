using Domain.Entities;

namespace Application.Services.Auth;

public interface IAuthService
{
    public AuthResponse? Login(string email, string password);
    public AuthResponse? Register(string name, string email, string password);
    public string? RefreshAccessToken(Guid id, string providedToken);
}