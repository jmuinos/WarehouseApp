using WarehouseApp.SharedKernel.Core.Primitives;

namespace WarehouseApp.SharedKernel.Core.Exceptions;

/// <summary>Represents an exception that occurred in the domain.</summary>
public class DomainException : Exception
{
    public Error Error { get; }
    
    /// <summary>Initializes a new instance of the <see cref="DomainException"/> class.</summary>
    /// <param name="error">The error containing the information about what happened.</param>
    public DomainException(Error error) : base(error.Message) => Error = error;

}