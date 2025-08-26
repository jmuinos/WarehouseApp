using WarehouseApp.SharedKernel.Core.Abstractions;
using WarehouseApp.SharedKernel.Core.Primitives;

namespace WarehouseApp.Domain.Companies;

/// <summary>Represents a company entity.</summary>
public sealed class Company : Entity, IAuditableEntity, ISoftDeletableEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string Address { get; private set; }
    public bool IsActive { get; private set; }

    public bool Deleted { get; }
    public DateTime? DeletedOnUtc { get; }

    public DateTime CreatedOnUtc { get; }
    public DateTime? ModifiedOnUtc { get; }

    // EF constructor
    private Company() { }

    public Company(string name, string address, string? description = null)
        : base(Guid.NewGuid())
    {
        Name = name;
        Address = address;
        Description = description;
        IsActive = true;
    }

    public void Update(string name, string address, string? description)
    {
        Name = name;
        Address = address;
        Description = description;
    }

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;
    
}