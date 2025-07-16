using Application.Features.Brands;
using FluentValidation;
using ResponseWrapperLibrary.Models.Requests.Products;

namespace Application.Features.Products.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator(IBrandService brandService)
    {
        RuleFor(x => x.BrandId)
            .Cascade(CascadeMode.Stop)
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