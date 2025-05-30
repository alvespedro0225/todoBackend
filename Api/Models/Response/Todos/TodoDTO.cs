using Domain.Enums;

namespace Api.Models.Response.Todos;

public sealed record TodoDto
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required Status Status { get; init; }
    public required Guid Id { get; init; }
}