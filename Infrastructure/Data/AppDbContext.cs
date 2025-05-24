using Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    
    public DbSet<TodoItem> Todos { get; set; }
    public DbSet<User> Users { get; set; }
}