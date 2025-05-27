namespace Domain.Entities;

public abstract class Entity : IEquatable<Entity>
{
    public required Guid Id { get; init; }

    public bool Equals(Entity? other)
    {
        return Equals((object?)other);
    }

    public override bool Equals(object? obj)
    {
        // two entities are the same when they have the same ID and are the of  same type 
        return obj is Entity entity && Id.Equals(entity.Id);
    }

    public static bool operator ==(Entity left, Entity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}