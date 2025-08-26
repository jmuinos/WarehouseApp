using MediatR;
using Microsoft.EntityFrameworkCore;
using WarehouseApp.Application.Abstractions.Data;
using WarehouseApp.Domain.Companies;
using WarehouseApp.SharedKernel.Core.Errors;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.GetById;

/// <summary>Handles the <see cref="GetCompanyByIdQuery"/>.</summary>
public sealed class GetCompanyByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetCompanyByIdQuery, Result<CompanyResponse>>
{
    public async Task<Result<CompanyResponse>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await dbContext.Set<Company>()
            .Where(c => c.Id == request.Id && !c.Deleted)
            .Select(c => new CompanyResponse(
                c.Id,
                c.Name,
                c.Address,
                c.Description,
                c.IsActive,
                c.CreatedOnUtc,
                c.ModifiedOnUtc))
            .FirstOrDefaultAsync(cancellationToken);

        if (company is null)
        {
            return Result.Failure<CompanyResponse>(DomainErrors.Company.NotFound);
        }

        return Result.Success(company);
    }
} 