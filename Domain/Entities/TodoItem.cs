namespace Domain.Entities;

public sealed record TodoItem
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
    public Status Status { get; set; } = Status.Todo;
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid Owner { get; init; }
};