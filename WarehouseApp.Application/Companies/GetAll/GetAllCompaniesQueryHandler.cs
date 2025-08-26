using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseApp.Application.Abstractions.Data;
using WarehouseApp.Domain.Companies;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.GetAll;

/// <summary>Handles the <see cref="GetAllCompaniesQuery"/>.</summary>
public sealed class GetAllCompaniesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetAllCompaniesQuery, Result<IEnumerable<CompanyResponse>>>
{
    public async Task<Result<IEnumerable<CompanyResponse>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await dbContext.Set<Company>()
            .Where(c => !c.Deleted)
            .Select(c => new CompanyResponse(
                c.Id,
                c.Name,
                c.Address,
                c.Description,
                c.IsActive,
                c.CreatedOnUtc,
                c.ModifiedOnUtc))
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<CompanyResponse>>(companies);
    }
} 