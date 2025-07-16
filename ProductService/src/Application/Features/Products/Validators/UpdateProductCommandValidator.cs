using Application.Features.Brands;
using Application.Features.Products.Commands;
using FluentValidation;

namespace Application.Features.Products.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(IProductService productService, IBrandService brandService)
    {
        RuleFor(x => x.UpdateProduct)
            .SetValidator(new UpdateProductRequestValidator(productService, brandService));
    }
}
