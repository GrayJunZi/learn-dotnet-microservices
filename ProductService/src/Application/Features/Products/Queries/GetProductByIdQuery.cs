using Mapster;
using MediatR;
using ResponseWrapperLibrary.Models.Responses.Products;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<IResponseWrapper>
{
    public int ProductId { get; set; }
}

public class GetProductByIdQueryHandler(IProductService productService) : IRequestHandler<GetProductByIdQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(request.ProductId);
        if (product == null || product.Id <= 0)
            return await ResponseWrapper.FailAsync("Product does not exists.");

        return await ResponseWrapper<ProductResponse>.SuccessAsync(product.Adapt<ProductResponse>());
    }
}
