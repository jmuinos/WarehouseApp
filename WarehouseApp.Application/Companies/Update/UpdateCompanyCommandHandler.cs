using MediatR;
using WarehouseApp.Application.Abstractions.Data;
using WarehouseApp.Domain.Companies;
using WarehouseApp.SharedKernel.Core.Errors;
using WarehouseApp.SharedKernel.Core.Primitives.Maybe;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.Update;

public sealed class UpdateCompanyCommandHandler(IApplicationDbContext dbContext, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCompanyCommand, Result>
{
    public async Task<Result> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyResult = await dbContext.FindByIdAsync<Company>(request.Id, cancellationToken);

        return await companyResult
                     .Ensure(c => !c.Deleted, DomainErrors.Company.NotFound)
                     .Tap(company =>
                     {
                         company.Update(request.Name, request.Address, request.Description);

                         if (request.IsActive)
                         {
                             company.Activate();
                         }
                         else
                         {
                             company.Deactivate();
                         }
                     })
                     .TapAsync(async _ => await unitOfWork.SaveChangesAsync(cancellationToken))
                     .Match(_ => Result.Success(), Result.Failure);
    }
}