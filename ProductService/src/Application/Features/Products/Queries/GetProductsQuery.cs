using Application.Pipelines;
using Mapster;
using MediatR;
using ResponseWrapperLibrary.Models.Responses.Products;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Products.Queries;

public class GetProductsQuery : IRequest<ResponseWrapper<List<ProductResponse>>>, ICacheable
{
    public string CacheKey { get; set; }
    public bool BypassCache { get; set; }
    public TimeSpan? SlidingExpiration { get; set; }

    public GetProductsQuery()
    {
        CacheKey = "GetProducts";
    }
}

public class GetProductsQueryHandler(IProductService productService) : IRequestHandler<GetProductsQuery, ResponseWrapper<List<ProductResponse>>>
{
    public async Task<ResponseWrapper<List<ProductResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productService.GetAllAsync();
        if (products == null || products.Count <= 0)
            return await ResponseWrapper<List<ProductResponse>>.FailAsync("No products were found.");

        return await ResponseWrapper<List<ProductResponse>>.SuccessAsync(products.Adapt<List<ProductResponse>>());
    }
}