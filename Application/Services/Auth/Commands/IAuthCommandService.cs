using Application.Common.Auth;
using Application.Common.Auth.Models.Requests;
using Application.Common.Auth.Models.Responses;

namespace Application.Services.Auth.Commands;

public interface IAuthCommandService
{
    public Task<AuthResponse> RegisterUser(RegisterCommandRequest registerCommandRequest);
    public Task<AuthResponse> Login(LoginCommandRequest loginCommandRequest);
    public Task DeleteUser(Guid userId);
}