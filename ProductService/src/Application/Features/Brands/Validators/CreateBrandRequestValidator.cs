using FluentValidation;
using ResponseWrapperLibrary.Models.Requests.Products;

namespace Application.Features.Brands.Validators;

public class CreateBrandRequestValidator : AbstractValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(60);

        RuleFor(x => x.Description)
            .MaximumLength(450);
    }
}
