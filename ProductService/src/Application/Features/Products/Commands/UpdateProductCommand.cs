using Application.Pipelines;
using Domain;
using Mapster;
using MediatR;
using ResponseWrapperLibrary.Models.Requests.Products;
using ResponseWrapperLibrary.Models.Responses.Products;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Products.Commands;

public class UpdateProductCommand : IRequest<IResponseWrapper>, IValidateSelf
{
    public UpdateProductRequest UpdateProduct { get; set; }
}

public class UpdateProductCommandHandler(IProductService productService) : IRequestHandler<UpdateProductCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productService.UpdateProductAsync(request.UpdateProduct.Adapt<Product>());
        return await ResponseWrapper<ProductResponse>
            .SuccessAsync(product.Adapt<ProductResponse>(),"Product updated successfully.");
    }
}
