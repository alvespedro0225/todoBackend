using Domain.Enums;

namespace Api1.Models.Request.Todos;

public sealed record TodoRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Status Status { get; set; }
}