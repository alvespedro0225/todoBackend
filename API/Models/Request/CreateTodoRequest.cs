using Domain;

namespace API.Models.Request;

public sealed record CreateTodoRequest(
    string Name,
    string Description,
    Status Status);