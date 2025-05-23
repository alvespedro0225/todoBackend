using Domain.Enums;

namespace Application.Common.Todos.Models.Requests;

public sealed record CreateTodoCommandRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Status Status { get; init; }
    public required Guid OwnerId { get; init; }
}