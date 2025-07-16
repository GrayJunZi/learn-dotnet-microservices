using Application.Features.Brands.Commands;
using Domain;
using FluentValidation;
using ResponseWrapperLibrary.Models.Requests.Products;

namespace Application.Features.Brands.Validators;

public class UpdateBrandRequestValidator : AbstractValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator(IBrandService brandService)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (id, ct) => await brandService.IsExistsAsync(id))
            .WithMessage("Brand does not exist.");
        RuleFor(x => x.Name)
            .MaximumLength(60);

        RuleFor(x => x.Description)
            .MaximumLength(450);
    }
}

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator(IBrandService brandService)
    {
        RuleFor(x => x.UpdateBrand)
            .SetValidator(new UpdateBrandRequestValidator(brandService));
    }
}