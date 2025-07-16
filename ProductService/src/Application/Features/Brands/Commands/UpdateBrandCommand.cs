using Application.Pipelines;
using Domain;
using Mapster;
using MediatR;
using ResponseWrapperLibrary.Models.Requests.Products;
using ResponseWrapperLibrary.Models.Responses.Products;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Brands.Commands;

public class UpdateBrandCommand : IRequest<IResponseWrapper>, IValidateSelf
{
    public UpdateBrandRequest UpdateBrand { get; set; }
}

public class UpdateBrandCommandHandler(IBrandService brandService) : IRequestHandler<UpdateBrandCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = new Brand
        {
            Id = request.UpdateBrand.Id,
            Name = request.UpdateBrand.Name,
            Description = request.UpdateBrand.Description,
        };
        var updatedBrand = await brandService.UpdateBrandAsync(brand);

        return await ResponseWrapper<BrandResponse>.SuccessAsync(updatedBrand.Adapt<BrandResponse>(), "Brand updated successfully.");
    }
}
