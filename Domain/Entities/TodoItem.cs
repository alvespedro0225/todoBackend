using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Domain.Enums;

namespace Domain.Entities;

public sealed class TodoItem : Entity
{
    [MaxLength(40)]
    public required string Name { get; set; }
    [MaxLength(40)]
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; set; }
    public required Status Status { get; set; }
    public Guid OwnerId { get; set; }
    public required User Owner { get; init; }
};