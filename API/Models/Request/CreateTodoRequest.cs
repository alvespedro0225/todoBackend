using Domain;

namespace API.Models.Request;

public sealed record CreateTodoRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public Status Status { get; init; }
}
