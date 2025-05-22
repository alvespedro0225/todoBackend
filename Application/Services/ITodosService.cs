using Application.Models.Request.Todos;
using Domain.Entities;

namespace Application.Services;

public interface ITodosService
{
    public List<TodoItem> GetTodos(Guid ownerId);
    public TodoItem GetTodo(Guid id);
    public TodoItem CreateTodoItem(Guid ownerId, TodosServiceRequest newTodosService);
    public TodoItem UpdateTodoItem(Guid id, TodosServiceRequest updatedTodos);
    public void DeleteTodoItem(Guid id);
}