using Application.Features.Brands;
using FluentValidation;
using ResponseWrapperLibrary.Models.Requests.Products;

namespace Application.Features.Products.Validators;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator(IProductService productService, IBrandService brandService)
    {
        RuleFor(x => x.Id)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (id, ct) => await productService.IsExistsAsync(id))
            .WithMessage("Product does not exists.");

        RuleFor(x => x.BrandId)
            .NotEmpty()
            .MustAsync(async (id, ct) => await brandService.IsExistsAsync(id))
            .WithMessage("Brand does not exists.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(60);

        RuleFor(x => x.ReOrderThreshHold)
            .NotEmpty();

        RuleFor(x => x.Quantity)
            .NotEmpty();
    }
}
