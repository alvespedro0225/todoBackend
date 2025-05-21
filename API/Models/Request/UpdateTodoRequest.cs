namespace API.Models.Request;

public sealed record UpdateTodoRequest
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid Owner { get; init; }
}
