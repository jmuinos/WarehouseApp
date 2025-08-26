namespace WarehouseApp.SharedKernel.Core.Primitives.Maybe;

/// <summary>Represents a wrapper around a value that may or may not be present.</summary>
/// <typeparam name="T">The wrapped value type.</typeparam>
public sealed class Maybe<T> : IEquatable<Maybe<T>>
{
    private readonly T? _value;

    public bool HasValue { get; }
    public bool HasNoValue => !HasValue;

    /// <summary>Initializes a new instance of the <see cref="Maybe{T}"/> class.</summary>
    /// <param name="value">The value to wrap (nullable).</param>
    public Maybe(T? value)
    {
        _value = value;
        HasValue = value is not null;
    }

    public T Value => HasValue ? _value! : throw new InvalidOperationException("No value present.");

    /// <summary>Gets an empty <see cref="Maybe{T}"/> instance.</summary>
    public static Maybe<T> None => new Maybe<T>(default);

    /// <summary>
    /// Creates a new <see cref="Maybe{T}"/> from a value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A new instance wrapping the value.</returns>
    public static Maybe<T> From(T value) => new Maybe<T>(value);

    public static implicit operator Maybe<T>(T value) => From(value);

    public static implicit operator T(Maybe<T> maybe) => maybe.Value;

    /// <inheritdoc />
    public bool Equals(Maybe<T>? other)
    {
        if (other is null) return false;
        if (HasNoValue && other.HasNoValue) return true;
        if (HasNoValue || other.HasNoValue) return false;

        return Value!.Equals(other.Value);
    }


    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj switch
        {
            null => false,
            T otherValue => Equals(new Maybe<T>(otherValue)),
            Maybe<T> maybe => Equals(maybe),
            _ => false
        };

    /// <inheritdoc />
    public override int GetHashCode() => HasValue ? _value!.GetHashCode() : 0;
}