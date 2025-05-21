namespace API.Models.Request;

public sealed record UpdateTodoRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid Owner { get; init; }
}
