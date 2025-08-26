using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseApp.Application.Abstractions.Data;
using WarehouseApp.Domain.Companies;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.GetActive;

/// <summary>Handles the <see cref="GetActiveCompaniesQuery"/>.</summary>
public sealed class GetActiveCompaniesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetActiveCompaniesQuery, Result<IEnumerable<CompanyResponse>>>
{
    public async Task<Result<IEnumerable<CompanyResponse>>> Handle(GetActiveCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await dbContext.Set<Company>()
            .Where(c => !c.Deleted && c.IsActive)
            .Select(c => new CompanyResponse(
                c.Id,
                c.Name,
                c.Address,
                c.Description,
                c.CreatedOnUtc,
                c.ModifiedOnUtc))
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<CompanyResponse>>(companies);
    }
} 