using Application.Common.Services;
using MediatR;
using ResponseWrapperLibrary.Models.Requests.Products.Events;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Products.Commands;

public class DeleteProductCommand : IRequest<IResponseWrapper>
{
    public int ProductId { get; set; }
}

public class DeleteProductCommandHandler(IProductService productService, IEventPublisher eventPublisher) : IRequestHandler<DeleteProductCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var productId = await productService.DeleteProductAsync(request.ProductId);
        if (productId == 0)
            return await ResponseWrapper.FailAsync($"Product with {request.ProductId} ID does not exists.");

        await eventPublisher.PublishAsync(new ProductDeletedEvent
        {
            Id = productId,
        }, cancellationToken);

        return await ResponseWrapper.SuccessAsync("Product deleted successfully.");
    }
}
