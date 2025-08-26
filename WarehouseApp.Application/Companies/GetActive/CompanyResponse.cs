namespace WarehouseApp.Application.Companies.GetActive;

/// <summary>Represents the company response.</summary>
public sealed record CompanyResponse(
    Guid Id,
    string Name,
    string Address,
    string? Description,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc); 