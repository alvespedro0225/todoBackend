using Application.Common.Todos;
using Application.Common.Todos.Models.Requests;
using Domain.Entities;

namespace Application.Services.Todos.Commands;

public interface ITodosCommandService
{
    public Task<TodoItem> CreateTodoItem(CreateTodoCommandRequest createTodoCommandRequest);
    public Task<TodoItem> UpdateTodoItem(Guid todoId, UpdateTodoCommandRequest updateTodoCommandRequest);
    public Task DeleteTodoItem(Guid todo);
}