using Application.Common.Repositories;
using Application.Services.Common;
using Domain.Entities;

namespace Application.Services.Todos.Queries;

public sealed class TodosQueryService(ITodoItemRepository todosRepository) : ITodosQueryService
{
    public List<TodoItem> GetTodos(Guid owner)
    {
        return todosRepository.GetTodos(owner);
    }

    public TodoItem? GetTodo(Guid todoId)
    {
        var todo = todosRepository.GetTodoItem(todoId);
        return todo;
    }
}