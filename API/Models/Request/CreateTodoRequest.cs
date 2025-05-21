using Domain;

namespace API.Models.Request;

public sealed record CreateTodoRequest
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public Status Status { get; init; }
}
