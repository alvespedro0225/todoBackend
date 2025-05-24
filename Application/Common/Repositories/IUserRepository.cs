using Domain.Entities;

namespace Application.Common.Repositories;

public interface IUserRepository
{
    public Task<User> GetUser(Guid userId);
    public Task<User> GetUser(string email);
    public Task AddUser(User user);
    public Task<bool> UserExists(Guid userId);
    public Task<bool> UserExists(string userEmail);
}