using Application.Common.Auth;

namespace Application.Services.Auth.Queries;

public interface IAuthQueryService
{
    public AuthResponse Login(string email, string password);
    public string RefreshAccessToken(Guid userId, string providedToken);
}