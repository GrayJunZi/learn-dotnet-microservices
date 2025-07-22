using Mapster;
using MediatR;
using ResponseWrapperLibrary.Models.Responses.Inventories;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.InventoryItems.Queries;

public class GetInventoryItemByIdQuery : IRequest<IResponseWrapper>
{
    public int ItemId { get; set; }
}

public class GetInventoryItemByIdQueryHandler(IInventoryItemService inventoryItemService) : IRequestHandler<GetInventoryItemByIdQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetInventoryItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await inventoryItemService.GetItemByIdAsync(request.ItemId);
        if (item == null)
            return await ResponseWrapper.FailAsync("Inventory Item does not exists.");

        var mappedItem = item.Adapt<InventoryItemResponse>();

        var productResponse = await inventoryItemService.GetProductByIdAsync(item.ProductId);
        if (!productResponse.IsSuccessful)
            return await ResponseWrapper<InventoryItemResponse>.SuccessAsync(mappedItem, productResponse.Messages);

        mappedItem.Product = productResponse.Data;
        return await ResponseWrapper<InventoryItemResponse>.SuccessAsync(mappedItem);
    }
}
