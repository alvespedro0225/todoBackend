using Application.Common.Todos.Models.Requests;

using Domain.Entities;

namespace Application.Common.Repositories;

public interface ITodoItemRepository
{
    public Task<List<TodoItem>> GetTodos(Guid ownerId);
    public Task<TodoItem> GetTodoItem(Guid todoId);
    public Task UpdateTodoItem(TodoItem todo);
    public Task DeleteTodoItem(Guid todoId);
    public Task AddTodoItem(TodoItem todo);
}