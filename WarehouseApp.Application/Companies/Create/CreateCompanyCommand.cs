using MediatR;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.Create;

/// <summary>Represents the command to create a company.</summary>
public sealed record CreateCompanyCommand(string Name, string Address, string? Description) : IRequest<Result<Guid>>;