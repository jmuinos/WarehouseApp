using MediatR;
using WarehouseApp.Application.Abstractions.Data;
using WarehouseApp.Domain.Companies;
using WarehouseApp.SharedKernel.Core.Primitives.Results;

namespace WarehouseApp.Application.Companies.Create;

/// <summary>Handles the <see cref="CreateCompanyCommand"/>.</summary>
public sealed class CreateCompanyCommandHandler(IApplicationDbContext dbContext, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCompanyCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = new Company(request.Name, request.Address, request.Description);

        dbContext.Add(company);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(company.Id);
    }
}
