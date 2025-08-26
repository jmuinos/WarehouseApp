using MediatR;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.GetActive;

/// <summary>Represents the query to get all active companies.</summary>
public sealed record GetActiveCompaniesQuery : IRequest<Result<IEnumerable<CompanyResponse>>>; 