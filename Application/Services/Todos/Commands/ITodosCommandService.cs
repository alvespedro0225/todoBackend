using Application.Common.Todos;
using Domain.Entities;

namespace Application.Services.Todos.Commands;

public interface ITodosCommandService
{
    public TodoItem CreateTodoItem(Guid ownerId, TodosServiceRequest newTodosService);
    public TodoItem UpdateTodoItem(TodoItem todo, TodosServiceRequest updatedTodo);
    public void DeleteTodoItem(TodoItem todo);
}