using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Todos.Models.Requests;

public sealed record UpdateTodoCommandRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public Status Status { get; init; }
}