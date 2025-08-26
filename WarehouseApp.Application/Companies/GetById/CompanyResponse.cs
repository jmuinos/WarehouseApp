namespace WarehouseApp.Application.Companies.GetById;

/// <summary>Represents the company response.</summary>
public sealed record CompanyResponse(
    Guid Id,
    string Name,
    string Address,
    string? Description,
    bool IsActive,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc); 