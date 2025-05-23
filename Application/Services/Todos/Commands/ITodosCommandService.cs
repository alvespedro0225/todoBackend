using Application.Common.Todos;
using Application.Common.Todos.Models.Requests;
using Domain.Entities;

namespace Application.Services.Todos.Commands;

public interface ITodosCommandService
{
    public TodoItem CreateTodoItem(CreateTodoCommandRequest createTodoCommandRequest);
    public TodoItem UpdateTodoItem(TodoItem todo, UpdateTodoCommandRequest updateTodoCommandRequest);
    public void DeleteTodoItem(TodoItem todo);
}