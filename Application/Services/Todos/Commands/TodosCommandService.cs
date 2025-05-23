using Application.Common.Repositories;
using Application.Common.Todos;
using Application.Common.Todos.Models.Requests;
using Application.Services.Common;
using Domain.Entities;

namespace Application.Services.Todos.Commands;

public sealed class TodosCommandService(
    ITodoItemRepository todosRepository,
    IDateTimeProvider dateTime) : ITodosCommandService
{
    public TodoItem UpdateTodoItem(
        TodoItem oldTodo,
        UpdateTodoCommandRequest updateTodoCommandRequest)
    {
        oldTodo.Name = updateTodoCommandRequest.Name;
        oldTodo.Status = updateTodoCommandRequest.Status;
        oldTodo.Description = updateTodoCommandRequest.Description;
        oldTodo.UpdatedAt = dateTime.Offset;
        return oldTodo;
    }

    public TodoItem CreateTodoItem(CreateTodoCommandRequest createTodoCommandRequest)
    {
        var now = dateTime.Offset;
        
        var todo = new TodoItem
        {
            Name = createTodoCommandRequest.Name,
            Description = createTodoCommandRequest.Description,
            CreatedAt = now,
            UpdatedAt = now,
            Status = createTodoCommandRequest.Status,
            UserId = createTodoCommandRequest.OwnerId
        };
        todosRepository.AddTodoItem(todo);
        return todo;
    }

    public void DeleteTodoItem(TodoItem todo)
    {
        todosRepository.DeleteTodoItem(todo);
    }
}