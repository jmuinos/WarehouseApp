using MediatR;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.GetAll;

/// <summary>Represents the query to get all companies.</summary>
public sealed record GetAllCompaniesQuery : IRequest<Result<IEnumerable<CompanyResponse>>>; 