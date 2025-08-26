using MediatR;
using WarehouseApp.Application.Abstractions.Data;
using WarehouseApp.Domain.Companies;
using WarehouseApp.SharedKernel.Core.Errors;
using WarehouseApp.SharedKernel.Core.Primitives.Maybe;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.Delete;

/// <summary>Handles the <see cref="DeleteCompanyCommand"/>.</summary>
public sealed class DeleteCompanyCommandHandler(IApplicationDbContext dbContext, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCompanyCommand, Result>
{
    public async Task<Result> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyResult = await dbContext.FindByIdAsync<Company>(request.Id, cancellationToken);

        return await companyResult
                     .Ensure(c => !c.Deleted, DomainErrors.Company.NotFound)
                     .TapAsync(async company =>
                     {
                         dbContext.Remove(company);
                         await unitOfWork.SaveChangesAsync(cancellationToken);
                     })
                     .Match(_ => Result.Success(), Result.Failure);
    }
} 