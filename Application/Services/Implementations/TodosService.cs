using Application.Common.Repositories;
using Application.Models.Request.Todos;
using Domain.Entities;

namespace Application.Services.Implementations;

public sealed class TodosService(
    ITodoItemRepository todosRepository,
    IDateTimeProvider dateTime) : ITodosService
{
    public List<TodoItem> GetTodos(Guid ownerId)
    {
        return todosRepository.GetTodos(ownerId);
    }

    public TodoItem GetTodo(Guid id)
    {
        var todo = todosRepository.GetTodoItem(id);
        
        if (todo is null)
            throw new Exception("Todo is null");

        return todo;
    }

    public TodoItem UpdateTodoItem(Guid id, TodosServiceRequest updatedTodos)
    {
        
        var todo = GetTodo(id);
        todo.Name = updatedTodos.Name;
        todo.Status = updatedTodos.Status;
        todo.Description = updatedTodos.Description;
        todo.UpdatedAt = dateTime.Offset;
        return todo;
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
            Owner = ownerId
        };
        todosRepository.AddTodoItem(todo);
        return todo;
    }

    public void DeleteTodoItem(Guid id)
    {
        var todo = GetTodo(id);
        todosRepository.DeleteTodoItem(todo);
    }
}