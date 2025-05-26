using Application.Common.Auth;
using Application.Common.Auth.Models.Requests;
using Application.Common.Auth.Models.Responses;

using Domain.Entities;

namespace Application.Services.Auth.Queries;

public interface IAuthQueryService
{
    public Task<string> RefreshAccessToken(RefreshCommandRequest refreshCommandRequest);
    public Task<User> GetUser(Guid userId);
}