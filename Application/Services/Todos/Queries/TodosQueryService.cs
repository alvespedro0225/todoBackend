using Application.Common.Repositories;
using Application.Services.Common;
using Domain.Entities;

namespace Application.Services.Todos.Queries;

public sealed class TodosQueryService(ITodoItemRepository todosRepository) : ITodosQueryService
{
    public async Task<List<TodoItem>> GetTodos(Guid owner)
    {
        return await todosRepository.GetTodos(owner);
    }

    public async Task<TodoItem> GetTodoItem(Guid todoId)
    {
        var todo = await todosRepository.GetTodoItem(todoId);
        return todo;
    }
}