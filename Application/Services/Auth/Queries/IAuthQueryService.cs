using Application.Common.Auth;
using Application.Common.Auth.Models.Requests;
using Application.Common.Auth.Models.Responses;

namespace Application.Services.Auth.Queries;

public interface IAuthQueryService
{
    public AuthResponse Login(AuthLoginRequest loginRequest);
    public string RefreshAccessToken(AuthRefreshRequest refreshRequest);
}