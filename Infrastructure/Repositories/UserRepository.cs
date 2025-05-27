using Application.Common.Repositories;

using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<User> GetUser(Guid userId)
    {
        var user = await dbContext.Users.FindAsync(userId);
        CheckForNull(user);
        return user!;
    }

    public async  Task<User> GetUser(string email)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        CheckForNull(user);
        return user!;
    }

    public async Task AddUser(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> UserExists(Guid userId)
    {
        return await dbContext.Users.AnyAsync(user => user.Id == userId);  
    }
    
    public async Task<bool> UserExists(string userEmail)
    {
        return await dbContext.Users.AnyAsync(user => user.Email == userEmail);  
    }

    public async Task DeleteUser(Guid userId)
    {
        var deleted = await dbContext.Users.Where(user => user.Id == userId).ExecuteDeleteAsync();
        
        if (deleted == 0)
            throw new NotFoundException(
                DefaultErrorMessages.UserNotFoundError,
                DefaultErrorMessages.UserNotFoundMessage);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateRefreshToken(string refreshToken, Guid userId)
    {
        var updated = await dbContext.Users.Where(user => user.Id == userId).ExecuteUpdateAsync(calls
            => calls.SetProperty(user => user.RefreshToken, refreshToken));
        
        if (updated == 0)
            throw new NotFoundException(
                DefaultErrorMessages.UserNotFoundError,
                DefaultErrorMessages.UserNotFoundMessage);

        await dbContext.SaveChangesAsync();
    }
    
    private static void CheckForNull(User? user)
    {
        if (user is null)
            throw new NotFoundException(
                DefaultErrorMessages.UserNotFoundError,
                DefaultErrorMessages.UserNotFoundMessage);
    }
        
}