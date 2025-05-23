using Application.Models.Response;
using Domain.Entities;

namespace Application.Services;

public interface IAuthService
{
    public AuthServiceResponse Login(string email, string password);
    public AuthServiceResponse Register(string name, string email, string password);
    public string RefreshAccessToken(Guid userId, string providedToken);
}