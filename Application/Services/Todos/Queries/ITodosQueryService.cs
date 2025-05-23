using Domain.Entities;

namespace Application.Services.Todos.Queries;

public interface ITodosQueryService
{
    public List<TodoItem> GetTodos(Guid ownerId);
    public TodoItem? GetTodo(Guid todoId);
}