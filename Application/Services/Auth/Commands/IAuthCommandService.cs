using Application.Common.Auth.Models.Requests;
using Application.Common.Auth.Models.Responses;

namespace Application.Services.Auth.Commands;

public interface IAuthCommandService
{
    public AuthResponse Register(AuthRegisterRequest registerRequest);
}