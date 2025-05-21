namespace Application.Common.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    public string GenerateAccessToken(Guid id, string name, string email);
    public string GenerateRefreshToken();
}