using Domain.Entities;

namespace Application.Common.Repositories;

public interface IUserRepository
{
    public User? GetUserFromId(Guid id);
    public User? GetUserFromEmail(string email);
    public void AddUser(User user);
}