using Application.Common.Auth;
using Application.Common.Auth.Models.Requests;
using Application.Common.Auth.Models.Responses;

namespace Application.Services.Auth.Queries;

public interface IAuthQueryService
{
    public Task<AuthResponse> Login(LoginCommandRequest loginCommandRequest);
    public Task<string> RefreshAccessToken(RefreshCommandRequest refreshCommandRequest);
}