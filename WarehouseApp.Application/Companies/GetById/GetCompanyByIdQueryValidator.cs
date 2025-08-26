using FluentValidation;

namespace WarehouseApp.Application.Companies.GetById;

/// <summary>Validates the <see cref="GetCompanyByIdQuery"/>.</summary>
public sealed class GetCompanyByIdQueryValidator : AbstractValidator<GetCompanyByIdQuery>
{
    public GetCompanyByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Company ID is required.");
    }
} 