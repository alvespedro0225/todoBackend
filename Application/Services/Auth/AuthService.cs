using Domain.Entities;

namespace Application.Services.Auth;

public sealed class AuthService : IAuthService
{
    private readonly List<User> _users = [];
    
    public AuthResponse? Login(string email, string password)
    {
        var user = _users.FirstOrDefault(listUser => listUser.Email == email);
        
        if (user is null)
            return null;

        var response = new AuthResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = "Token"
        };
        
        return user.Password == password ? response : null;
    }

    public AuthResponse? Register(string name, string email, string password)
    {
        if (_users.FirstOrDefault(user => user.Email == email) is not null)
            return null;
        var id = Guid.NewGuid();
        var response = new AuthResponse
        {
            Name = name,
            Email = email,
            Id = id,
            Token = "token"
        };

        var user = new User
        {
            Name = name,
            Email = email,
            Id = id,
            Password = password
        };
        _users.Add(user);
        return response;
    }
}