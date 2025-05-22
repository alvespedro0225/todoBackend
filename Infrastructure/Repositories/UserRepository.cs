using Application.Common.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private static readonly List<User> Users = []; 
    
    public User? GetUserFromId(Guid id)
    {
        return Users.FirstOrDefault(user => user.Id == id);
    }

    public User? GetUserFromEmail(string email)
    {
        return Users.FirstOrDefault(user => user.Email == email);
    }

    public void AddUser(User user)
    {
        Users.Add(user);
    }
}