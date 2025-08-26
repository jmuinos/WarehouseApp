namespace WarehouseApp.SharedKernel.Core.Primitives;
/// <summary>Represents the base class from which all value objects derive.</summary>
public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
{
    public static bool operator ==(ValueObject<T>? a, ValueObject<T>? b)
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

    public static bool operator !=(ValueObject<T> a, ValueObject<T> b) => !(a == b);

    public bool Equals(T? other)
    {
        return other is not null &&
               GetType() == other.GetType() &&
               GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }

    public override bool Equals(object? obj) =>
        obj is T valueObject && Equals(valueObject);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (var obj in GetAtomicValues())
        {
            hashCode.Add(obj);
        }

        return hashCode.ToHashCode();
    }

    /// <summary>Gets the atomic values that define the equality of the value object.</summary>
    /// <returns>Enumerable of objects that participate in equality.</returns>
    protected abstract IEnumerable<object> GetAtomicValues();
}