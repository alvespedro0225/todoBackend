using System.Security.Claims;
using API.Utilities;
using API.Validators.Todos;
using Application.Common.Exceptions;
using Application.Common.Todos;
using Application.Services;
using Application.Services.Todos.Commands;
using Application.Services.Todos.Queries;
using Domain.Entities;
using FluentValidation;

namespace API.Endpoints;

public static class Todos
{
    public static void MapTodosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("todos")
            .RequireAuthorization();
        group.MapGet("", GetTodos);
        group.MapGet("{todoId:guid}", GetTodoItem);
        group.MapPost("", CreateTodoItem);
        group.MapPut("{todoId:guid}", UpdateTodoItem);
        group.MapDelete("{todoId:guid}", DeleteTodoItem);
    }

    public static IResult GetTodos(
        ITodosQueryService todosQueryService,
        HttpContext context)
    {
        var userId = GetUserId(context);
        var todos = todosQueryService.GetTodos(userId);

        return Results.Ok(todos);
    }

    public static IResult GetTodoItem(
        ITodosQueryService todosQueryService,
        HttpContext context,
        Guid todoId)
    {
        var todo = todosQueryService.GetTodo(todoId);
        VerifyTodo(todo, context);
        return Results.Ok(todo);
    }

    public static IResult CreateTodoItem(
        ITodosCommandService todosCommandService,
        TodosServiceRequest requestTodo,
        HttpContext context)
    {
        ValidateTodo(new TodoRequestValidator(), requestTodo);
        var userId = GetUserId(context);
        var todo = todosCommandService.CreateTodoItem(userId, requestTodo);
        return Results.Ok(todo);
    }

    public static IResult UpdateTodoItem(
        ITodosQueryService todosQueryService,
        ITodosCommandService todosCommandService,
        HttpContext context,
        TodosServiceRequest requestTodo,
        Guid todoId)
    {
        ValidateTodo(new TodoRequestValidator(), requestTodo);
        var todo = todosQueryService.GetTodo(todoId);
        VerifyTodo(todo, context);
        todo = todosCommandService.UpdateTodoItem(todo!, requestTodo);
        return Results.Ok(todo);
    }

    public static IResult DeleteTodoItem(
        ITodosQueryService todosQueryService,
        ITodosCommandService todosCommandService,
        HttpContext context,
        Guid todoId)
    {
        var todo = todosQueryService.GetTodo(todoId);
        VerifyTodo(todo, context);
        todosCommandService.DeleteTodoItem(todo!);
        return Results.NoContent();
    }

    private static Guid GetUserId(HttpContext context)
    {
        var user = context.User;
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            throw new UnauthorizedException(
                "Missing Name Identifier Claim",
                "Make sure this is a registered user");

        var stringUserId = userIdClaim.Value;

        if (!Guid.TryParse(stringUserId, out var userId))
            throw new UnauthorizedException(
                "Invalid Name Identifier Claim",
                "Make sure user has a valid Guid registered");

        return userId;
    }

    private static void ValidateTodo<T>(AbstractValidator<T> validator, T todo)
    {
        validator.ValidateAndThrow(todo);
    }
    
    private static void VerifyTodo(TodoItem? todo, HttpContext context)
    {
        if (todo is null)
            throw new NotFoundException(
                DefaultErrorMessages.TodoNotFoundError,
                DefaultErrorMessages.TodoNotFoundMessage);
        
        var userId = GetUserId(context);
        
        if (todo.UserId != userId)
            throw new ForbiddenException(
                DefaultErrorMessages.ForbiddenError,
                DefaultErrorMessages.ForbiddenTodoMessage);
    }
}