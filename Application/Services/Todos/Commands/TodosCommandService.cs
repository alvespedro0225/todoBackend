using Application.Common.Repositories;
using Application.Common.Todos;
using Application.Services.Common;
using Domain.Entities;

namespace Application.Services.Todos.Commands;

public sealed class TodosCommandService(
    ITodoItemRepository todosRepository,
    IDateTimeProvider dateTime) : ITodosCommandService
{
    public TodoItem UpdateTodoItem(TodoItem oldTodo, TodosServiceRequest updatedTodo)
    {
        oldTodo.Name = updatedTodo.Name;
        oldTodo.Status = updatedTodo.Status;
        oldTodo.Description = updatedTodo.Description;
        oldTodo.UpdatedAt = dateTime.Offset;
        return oldTodo;
    }

    public TodoItem CreateTodoItem(Guid ownerId, TodosServiceRequest newTodosService)
    {
        var now = dateTime.Offset;
        
        var todo = new TodoItem
        {
            Name = newTodosService.Name,
            Description = newTodosService.Description,
            CreatedAt = now,
            UpdatedAt = now,
            Status = newTodosService.Status,
            UserId = ownerId
        };
        todosRepository.AddTodoItem(todo);
        return todo;
    }

    public void DeleteTodoItem(TodoItem todo)
    {
        todosRepository.DeleteTodoItem(todo);
    }
}