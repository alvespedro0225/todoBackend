using Domain.Entities;

namespace Application.Common.Repositories;

public interface ITodoItemRepository
{
    public List<TodoItem> GetTodos(Guid ownerId);
    public TodoItem? GetTodoItem(Guid todoId);
    public void UpdateTodoItem(TodoItem oldTodo, TodoItem updatedTodo);
    public void DeleteTodoItem(TodoItem todo);
    public void AddTodoItem(TodoItem todo);
}