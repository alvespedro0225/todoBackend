using System.Security.Claims;
using API.Utilities;
using API.Validators.Todos;
using Application.Common.Repositories;
using Application.Models.Request.Todos;
using Application.Services;
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
        group.MapPost("{todoId:guid}", UpdateTodoItem);
        group.MapDelete("{todoId:guid}", DeleteTodoItem);
    }

    public static IResult GetTodos(
        ITodosService todosService,
        HttpContext context)
    {
        var userId = GetUserId(context);
        var todos = todosService.GetTodos(userId);

        return Results.Ok(todos);
    }

    public static IResult GetTodoItem(
        ITodosService todosService,
        HttpContext context,
        Guid todoId)
    {
        var todo = todosService.GetTodo(todoId);
        VerifyTodo(todo, context);
        return Results.Ok(todo);
    }

    public static IResult CreateTodoItem(
        ITodosService todosService,
        IUserRepository userRepository,
        TodosServiceRequest requestTodo,
        HttpContext context)
    {
        ValidateTodo(new TodoRequestValidator(), requestTodo);
        var userId = GetUserId(context);
        var user = userRepository.GetUser(userId);

        if (user is null)
            throw new NotFoundException(
                DefaultErrorMessages.UserNotFoundError,
                DefaultErrorMessages.UserNotFoundMessage);

        var todo = todosService.CreateTodoItem(user, requestTodo);
        return Results.Ok(todo);
    }

    public static IResult UpdateTodoItem(
        ITodosService todosService,
        HttpContext context,
        TodosServiceRequest requestTodo,
        Guid todoId)
    {
        ValidateTodo(new TodoRequestValidator(), requestTodo);
        var todo = todosService.GetTodo(todoId);
        todo = todosService.UpdateTodoItem(todo!, requestTodo);
        return Results.Ok(todo);
    }

    public static IResult DeleteTodoItem(
        ITodosService todosService,
        HttpContext context,
        Guid todoId)
    {
        var todo = todosService.GetTodo(todoId);
        VerifyTodo(todo, context);
        todosService.DeleteTodoItem(todo!);
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
        
        if (todo.Owner.Id != userId)
            throw new ForbiddenException(
                DefaultErrorMessages.ForbiddenError,
                DefaultErrorMessages.ForbiddenTodoMessage);
    }
}