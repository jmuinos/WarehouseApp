using MediatR;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.GetById;

/// <summary>Represents the query to get a company by ID.</summary>
public sealed record GetCompanyByIdQuery(Guid Id) : IRequest<Result<CompanyResponse>>; 