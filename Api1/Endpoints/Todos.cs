using System.Security.Claims;

using Api1.Models.Response;
using Api1.Models.Request.Todos;
using Api1.Models.Response.Todos;
using Api1.Validators.Todos;

using Application.Common.Todos.Models.Requests;
using Application.Services.Auth.Queries;
using Application.Services.Todos.Commands;
using Application.Services.Todos.Queries;

using Domain.Constants;
using Domain.Entities;
using Domain.Exceptions;

using FluentValidation;

namespace Api1.Endpoints;

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
        var todosDto = todos.Select(ConvertToDto).ToList();
        return TypedResults.Ok(todosDto);
    }

    public static async Task<IResult> GetTodoItem(
        ITodosQueryService todosQueryService,
        HttpContext context,
        Guid todoId)
    {
        var todo = await todosQueryService.GetTodoItem(todoId);
        ValidateOwnership(todo.OwnerId, context);
        var todoDto = ConvertToDto(todo);
        return TypedResults.Ok(todoDto);
    }

    public async static Task<IResult> CreateTodoItem(
        ITodosCommandService todosCommandService,
        IAuthQueryService authQueryService,
        TodoRequest todoRequest,
        HttpContext context)
    {
        ValidateTodo(new TodoRequestValidator(), todoRequest);
        var userId = GetUserId(context);
        var user = await authQueryService.GetUser(userId);
        
        var todo = await todosCommandService.CreateTodoItem(new CreateTodoCommandRequest
        {
            Owner = user,
            Name = todoRequest.Name,
            Description = todoRequest.Description,
            Status = todoRequest.Status
        });
        
        var todoDto = ConvertToDto(todo);
        return TypedResults.Created($"todos/{todo.Id}", todoDto);
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
        
        ValidateOwnership(todo.OwnerId, context);
        var todoDto = ConvertToDto(todo);
        return TypedResults.Ok(todoDto);
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
        return TypedResults.NoContent();
    }

    private static Guid GetUserId(HttpContext context)
    {
        var user = context.User;
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            throw new UnauthorizedException(
                DefaultErrorMessages.UnauthorizedMissingClaimError,
                DefaultErrorMessages.UnauthorizedMissingClaimMessage);

        var stringUserId = userIdClaim.Value;

        if (!Guid.TryParse(stringUserId, out var userId))
            throw new UnauthorizedException(
                DefaultErrorMessages.UnauthorizedInvalidClaimError,
                DefaultErrorMessages.UnauthorizedInvalidClaimMessage);

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

    private static TodoDto ConvertToDto(TodoItem todo)
    {
        var todoDto = new TodoDto
        {
            Name = todo.Name,
            Description = todo.Description,
            CreatedAt = todo.CreatedAt,
            UpdatedAt = todo.UpdatedAt,
            Status = todo.Status,
            Id = todo.Id
        };
        return todoDto;
    }
}