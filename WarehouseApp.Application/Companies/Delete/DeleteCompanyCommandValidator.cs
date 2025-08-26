using FluentValidation;

namespace WarehouseApp.Application.Companies.Delete;

/// <summary>Validates the <see cref="DeleteCompanyCommand"/>.</summary>
public sealed class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Company ID is required.");
    }
} 