using MediatR;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.Delete;

/// <summary>Represents the command to delete a company.</summary>
public sealed record DeleteCompanyCommand(Guid Id) : IRequest<Result>; 