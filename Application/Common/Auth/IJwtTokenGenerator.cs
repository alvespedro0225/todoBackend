using Domain.Entities;

namespace Application.Common.Auth;

public interface IJwtTokenGenerator
{
    public string GenerateAccessToken(User user);
    public string GenerateRefreshToken();
}