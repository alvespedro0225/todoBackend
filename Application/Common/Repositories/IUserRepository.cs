using Domain.Entities;

namespace Application.Common.Repositories;

public interface IUserRepository
{
    public User? GetUser(Guid id);
    public User? GetUser(string email);
    public void AddUser(User user);
}