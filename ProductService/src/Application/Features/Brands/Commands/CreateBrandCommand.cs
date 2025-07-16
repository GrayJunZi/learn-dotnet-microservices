using Application.Pipelines;
using Domain;
using MediatR;
using ResponseWrapperLibrary.Models.Requests.Products;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Brands.Commands;

public class CreateBrandCommand : IRequest<IResponseWrapper>, IValidateSelf
{
    public CreateBrandRequest CreateBrand { get; set; }
}

public class CreateBrandCommandHandler(IBrandService brandService) : IRequestHandler<CreateBrandCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = new Brand
        {
            Name = request.CreateBrand.Name,
            Description = request.CreateBrand.Description,
        };

        var brandId = await brandService.CreateBrandAsync(brand);

        return await ResponseWrapper<int>.SuccessAsync(brandId, "Brand created successfully.");
    }
}