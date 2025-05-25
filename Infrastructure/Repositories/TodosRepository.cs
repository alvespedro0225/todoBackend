using Application.Common.Repositories;

using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class TodosRepository(AppDbContext dbContext) : ITodoItemRepository
{
    
    public async Task<List<TodoItem>> GetTodos(Guid ownerId)
    {
        return await dbContext.Todos.AsNoTracking().Where(todo => todo.Owner.Id == ownerId).ToListAsync();
    }

    public async Task<TodoItem> GetTodoItem(Guid todoId)
    {
        var todo = await dbContext.Todos.FindAsync(todoId);

        if (todo is null)
            throw new NotFoundException(
                DefaultErrorMessages.TodoNotFoundError,
                DefaultErrorMessages.TodoNotFoundMessage);

        return todo;
    }

    public async Task UpdateTodoItem(TodoItem todo)
    {
        dbContext.Todos.Update(todo);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteTodoItem(Guid todoId)
    {
        if (!await dbContext.Todos.AnyAsync(todo => todo.Id == todoId))
            throw new NotFoundException(
                DefaultErrorMessages.TodoNotFoundError,
                DefaultErrorMessages.TodoNotFoundMessage);
        
        await dbContext.Todos.Where(todo => todo.Id == todoId).ExecuteDeleteAsync();
        await dbContext.SaveChangesAsync();
    }

    public async Task AddTodoItem(TodoItem todo)
    {
        dbContext.Add(todo);
        await dbContext.SaveChangesAsync();
    }
}