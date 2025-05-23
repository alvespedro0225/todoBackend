using Application.Common.Todos;
using Domain.Entities;

namespace Application.Services;

public interface ITodosService
{
    public List<TodoItem> GetTodos(Guid ownerId);
    public TodoItem? GetTodo(Guid todoId);
    public TodoItem CreateTodoItem(Guid ownerId, TodosServiceRequest newTodosService);
    public TodoItem UpdateTodoItem(TodoItem todo, TodosServiceRequest updatedTodo);
    public void DeleteTodoItem(TodoItem todo);
}