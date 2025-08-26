using FluentValidation;

namespace WarehouseApp.Application.Companies.Update;

public sealed class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(100);
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.").MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}