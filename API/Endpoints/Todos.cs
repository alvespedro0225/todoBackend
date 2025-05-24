using System.Security.Claims;
using API.Models.Request.Todos;
using API.Utilities;
using API.Validators.Todos;
using Application.Common.Todos.Models.Requests;
using Application.Services.Todos.Commands;
using Application.Services.Todos.Queries;
using Domain.Entities;
using Domain.Exceptions;
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

    public async static Task<IResult> GetTodos(
        ITodosQueryService todosQueryService,
        HttpContext context)
    {
        var userId = GetUserId(context);
        var todos = await todosQueryService.GetTodos(userId);

        return Results.Ok(todos);
    }

    public static async Task<IResult> GetTodoItem(
        ITodosQueryService todosQueryService,
        HttpContext context,
        Guid todoId)
    {
        var todo = await todosQueryService.GetTodoItem(todoId);
        ValidateOwnership(todo.Owner, context);
        return Results.Ok(todo);
    }

    public static IResult CreateTodoItem(
        ITodosCommandService todosCommandService,
        TodoRequest todoRequest,
        HttpContext context)
    {
        ValidateTodo(new TodoRequestValidator(), todoRequest);
        var userId = GetUserId(context);
        
        var todo = todosCommandService.CreateTodoItem(new CreateTodoCommandRequest
        {
            OwnerId = userId,
            Name = todoRequest.Name,
            Description = todoRequest.Description,
            Status = todoRequest.Status
        });
        
        return Results.Created($"todos/{todo.Id}", todo);
    }

    public static async Task<IResult> UpdateTodoItem(
        ITodosCommandService todosCommandService,
        HttpContext context,
        TodoRequest todoRequest,
        Guid todoId)
    {
        ValidateTodo(new TodoRequestValidator(), todoRequest);

        var todo = await todosCommandService.UpdateTodoItem(todoId, new UpdateTodoCommandRequest
        {
            Name = todoRequest.Name,
            Description = todoRequest.Description,
            Status = todoRequest.Status
        });
        
        ValidateOwnership(todo.Owner, context);
        
        return Results.Ok(todo);
    }

    public static async Task<IResult> DeleteTodoItem(
        ITodosQueryService todosQueryService,
        ITodosCommandService todosCommandService,
        HttpContext context,
        Guid todoId)
    {
        var todo = await todosQueryService.GetTodoItem(todoId);
        ValidateOwnership(todo.Id, context);
        await todosCommandService.DeleteTodoItem(todoId);
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
    
    private static void ValidateOwnership(Guid todoOwner, HttpContext context)
    {
        var userId = GetUserId(context);
        if (todoOwner != userId)
            throw new ForbiddenException(
                DefaultErrorMessages.ForbiddenError,
                DefaultErrorMessages.ForbiddenTodoMessage);
    }
}