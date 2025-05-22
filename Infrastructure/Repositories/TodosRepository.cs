using Application.Common.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class TodosRepository : ITodoItemRepository
{
    private static readonly List<TodoItem> Todos = [];
    
    public List<TodoItem> GetTodos(Guid ownerId)
    {
        return Todos.Where(todo => todo.Owner == ownerId).ToList();
    }

    public TodoItem? GetTodoItem(Guid id)
    {
        return Todos.FirstOrDefault(todo => todo.Id == id);
    }

    public void UpdateTodoItem(TodoItem oldTodo, TodoItem updatedTodo)
    {
        var index = Todos.IndexOf(oldTodo);
        Todos[index] = updatedTodo;
    }

    public void DeleteTodoItem(TodoItem todo)
    {
        Todos.Remove(todo);
    }

    public void AddTodoItem(TodoItem todo)
    {
        Todos.Add(todo);
    }
}