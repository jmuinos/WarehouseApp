using MediatR;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.Update;

public sealed record UpdateCompanyCommand(Guid Id, string Name, string Address, string? Description, bool IsActive) : IRequest<Result>;