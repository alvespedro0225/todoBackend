using System.Security.Claims;
using API.Validators.Todos;
using Application.Models.Request.Todos;
using Application.Services;
using FluentValidation;
using FluentValidation.Results;

namespace API.Endpoints;

public static class Todos
{
    public static void MapTodosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("todos");
        group.MapGet("", GetTodos).RequireAuthorization();
        group.MapGet("{todoId:guid}", GetTodoItem).RequireAuthorization();
        group.MapPost("", CreateTodoItem).RequireAuthorization();
        group.MapPost("{todoId:guid}", UpdateTodoItem);
        group.MapDelete("{todoId:guid}", DeleteTodoItem);
    }

    public static IResult GetTodos(ITodosService todosService, HttpContext context)
    {
        var userId = GetUserId(context);
        var todos = todosService.GetTodos(userId);
        return Results.Ok(todos);
    }

    public static IResult GetTodoItem(ITodosService todosService, Guid todoId)
    {
        var todo = todosService.GetTodo(todoId);
        return Results.Ok(todo);
    }

    public static IResult CreateTodoItem(ITodosService todosService, TodosServiceRequest requestTodo, HttpContext context)
    {
        var validationResult = ValidateTodo(
            new TodoRequestValidator(),
            requestTodo,
            out var errors);

        if (!validationResult)
            return Results.BadRequest(errors);

        var userId = GetUserId(context);
        var todo = todosService.CreateTodoItem(userId, requestTodo);
        return Results.Ok(todo);
    }

    public static IResult UpdateTodoItem(ITodosService todosService, TodosServiceRequest requestTodo, Guid todoId)
    {
        var validationResult = ValidateTodo(
            new TodoRequestValidator(),
            requestTodo,
            out var errors);

        if (!validationResult)
            return Results.BadRequest(errors);
        
        var todo = todosService.UpdateTodoItem(todoId, requestTodo);
        return Results.Ok(todo);
    }

    public static IResult DeleteTodoItem(ITodosService todosService, Guid todoId)
    {
        todosService.DeleteTodoItem(todoId);
        return Results.NoContent();
    }

    private static Guid GetUserId(HttpContext context)
    {
        var user = context.User;
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        
        if (userIdClaim is null)
            throw new Exception("Missing \"Name Identifier\" claim");

        var stringUserId = userIdClaim.Value;
        if (!Guid.TryParse(stringUserId, out var userId))
            throw new Exception("Couldn't convert claim to guid");

        return userId;
    }

    private static bool ValidateTodo<T>(AbstractValidator<T> validator, T todo, out List<ValidationFailure> errors)
    {
        var validationResult = validator.Validate(todo);
        errors = validationResult.Errors;
        return validationResult.IsValid;
    }
}