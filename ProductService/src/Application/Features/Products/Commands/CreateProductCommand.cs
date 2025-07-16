using Application.Common.Services;
using Application.Pipelines;
using Domain;
using Mapster;
using MediatR;
using ResponseWrapperLibrary.Models.Requests.Products;
using ResponseWrapperLibrary.Models.Requests.Products.Events;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Products.Commands;

public class CreateProductCommand : IRequest<IResponseWrapper>, IValidateSelf
{
    public CreateProductRequest CreateProduct { get; set; }
}

public class CreateProductCommandHandler(IProductService productService, IEventPublisher eventPublisher) : IRequestHandler<CreateProductCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productId = await productService.CreateProductAsync(request.CreateProduct.Adapt<Product>());

        await eventPublisher.PublishAsync(new ProductCreatedEvent
        {
            Id = productId,
            Name = request.CreateProduct.Name,
            ReOrderThreshHold = request.CreateProduct.ReOrderThreshHold,
            Quantity = request.CreateProduct.Quantity,
        }, cancellationToken);

        return await ResponseWrapper<int>.SuccessAsync(productId, "Product created successfully.");
    }
}
