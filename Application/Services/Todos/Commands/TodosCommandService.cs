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
    public async Task<TodoItem> UpdateTodoItem(
        Guid todoId,
        UpdateTodoCommandRequest updateTodoCommandRequest)
    {
        var todo = await todosRepository.GetTodoItem(todoId);

        todo.Name = updateTodoCommandRequest.Name;
        todo.Description = updateTodoCommandRequest.Description;
        todo.Status = updateTodoCommandRequest.Status;
        todo.UpdatedAt = dateTime.UtcNow;
        
        await todosRepository.UpdateTodoItem(todo);
        return todo;
    }

    public async Task<TodoItem> CreateTodoItem(CreateTodoCommandRequest createTodoCommandRequest)
    {
        var now = dateTime.Offset;
        
        var todo = new TodoItem
        {
            Id = Guid.NewGuid(),
            Name = createTodoCommandRequest.Name,
            Description = createTodoCommandRequest.Description,
            CreatedAt = now,
            UpdatedAt = now,
            Status = createTodoCommandRequest.Status,
            Owner = createTodoCommandRequest.Owner 
        };
        await todosRepository.AddTodoItem(todo);
        return todo;
    }

    public async Task DeleteTodoItem(Guid todoId)
    {
        await todosRepository.DeleteTodoItem(todoId);
    }
}