using WarehouseApp.Application.Abstractions.Date;

namespace WarehouseApp.Infrastructure;

/// <summary>Provides the current date and time.</summary>
public class DateTimeProvider : IDateTime
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
} 