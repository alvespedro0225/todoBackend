using Application.Models.Request.Todos;
using Domain.Entities;

namespace Application.Services;

public interface ITodosService
{
    public List<TodoItem> GetTodos(Guid ownerId);
    public TodoItem? GetTodo(Guid id);
    public TodoItem CreateTodoItem(User owner, TodosServiceRequest newTodosService);
    public TodoItem UpdateTodoItem(TodoItem todo, TodosServiceRequest updatedTodo);
    public void DeleteTodoItem(TodoItem todo);
}