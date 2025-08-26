using WarehouseApp.SharedKernel.Core.Guards;

namespace WarehouseApp.SharedKernel.Core.Primitives;

/// <summary>Represents the base class that all entities derive from.</summary>
public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; private init; }
    
    protected Entity(Guid id) : this()
    {
        Ensure.NotEmpty(id, "The identifier is required.", nameof(id));
        Id = id;
    }
    
    protected Entity() { }
    
    public static bool operator ==(Entity? a, Entity? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity? a, Entity? b) => !(a == b);

    /// <inheritdoc />
    public bool Equals(Entity? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id == other.Id;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return obj is Entity other && Id == other.Id;
    }
    
    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode() * 41;
}