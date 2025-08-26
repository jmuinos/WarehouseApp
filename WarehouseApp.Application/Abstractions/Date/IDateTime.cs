namespace WarehouseApp.Application.Abstractions.Date
{
    /// <summary>Represents the interface for getting the current date and time.</summary>
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}