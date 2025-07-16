using Application.Features.Brands.Commands;
using FluentValidation;

namespace Application.Features.Brands.Validators;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.CreateBrand)
            .SetValidator(new CreateBrandRequestValidator());
    }
}