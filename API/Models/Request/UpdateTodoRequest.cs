namespace API.Models.Request;

public sealed record UpdateTodoRequest(
    string Name,
    string Description,
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid Owner);