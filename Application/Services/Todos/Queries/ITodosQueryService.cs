using Domain.Entities;

namespace Application.Services.Todos.Queries;

public interface ITodosQueryService
{
    public Task<List<TodoItem>> GetTodos(Guid ownerId);
    public Task<TodoItem> GetTodoItem(Guid todoId);
}