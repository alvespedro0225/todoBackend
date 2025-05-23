using Application.Common.Repositories;
using Application.Models.Request.Todos;
using Domain.Entities;

namespace Application.Services.Implementations;

public sealed class TodosService(
    ITodoItemRepository todosRepository,
    IDateTimeProvider dateTime) : ITodosService
{
    public List<TodoItem> GetTodos(User owner)
    {
        return todosRepository.GetTodos(owner);
    }

    public TodoItem? GetTodo(Guid id)
    {
        var todo = todosRepository.GetTodoItem(id);
        return todo;
    }

    public TodoItem UpdateTodoItem(TodoItem oldTodo, TodosServiceRequest updatedTodo)
    {
        oldTodo.Name = updatedTodo.Name;
        oldTodo.Status = updatedTodo.Status;
        oldTodo.Description = updatedTodo.Description;
        oldTodo.UpdatedAt = dateTime.Offset;
        return oldTodo;
    }

    public TodoItem CreateTodoItem(User owner, TodosServiceRequest newTodosService)
    {
        var now = dateTime.Offset;
        
        var todo = new TodoItem
        {
            Name = newTodosService.Name,
            Description = newTodosService.Description,
            CreatedAt = now,
            UpdatedAt = now,
            Status = newTodosService.Status,
            Owner = owner
        };
        todosRepository.AddTodoItem(todo);
        return todo;
    }

    public void DeleteTodoItem(TodoItem todo)
    {
        todosRepository.DeleteTodoItem(todo);
    }
}