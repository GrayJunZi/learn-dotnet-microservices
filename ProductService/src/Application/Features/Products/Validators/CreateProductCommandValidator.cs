using Application.Features.Brands;
using Application.Features.Products.Commands;
using FluentValidation;

namespace Application.Features.Products.Validators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IBrandService brandService)
    {
        RuleFor(x => x.CreateProduct)
            .SetValidator(new CreateProductRequestValidator(brandService));
    }
}
