using Application.Common.Auth;

namespace Application.Services.Auth.Commands;

public interface IAuthCommandService
{
    public AuthResponse Register(string name, string email, string password);
}