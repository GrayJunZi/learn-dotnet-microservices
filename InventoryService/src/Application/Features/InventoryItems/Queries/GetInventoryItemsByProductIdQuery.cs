using Mapster;
using MediatR;
using ResponseWrapperLibrary.Models.Responses.Inventories;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.InventoryItems.Queries;

public class GetInventoryItemsByProductIdQuery : IRequest<IResponseWrapper>
{
    public int ProductId { get; set; }
}

public class GetInventoryItemsByProductIdQueryHandler (IInventoryItemService inventoryItemService): IRequestHandler<GetInventoryItemsByProductIdQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetInventoryItemsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var inventoryItems = await inventoryItemService.GetItemsByProductIdAsync(request.ProductId);
        if (inventoryItems == null || inventoryItems.Count <= 0)
            return await ResponseWrapper.FailAsync("Inventory Items does not exists.");

        return await ResponseWrapper<List<InventoryItemResponse>>.SuccessAsync(inventoryItems.Adapt<List<InventoryItemResponse>>());
    }
}