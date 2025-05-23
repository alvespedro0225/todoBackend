using Domain;
using Domain.Enums;

namespace Application.Models.Request.Todos;

public sealed record TodosServiceRequest
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public Status Status { get; init; }
}
