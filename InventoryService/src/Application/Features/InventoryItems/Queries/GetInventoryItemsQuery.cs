using Mapster;
using MediatR;
using ResponseWrapperLibrary.Models.Responses.Inventories;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.InventoryItems.Queries;

public class GetInventoryItemsQuery : IRequest<IResponseWrapper>
{

}

public class GetInventoryItemsQueryHandler(IInventoryItemService inventoryItemService) : IRequestHandler<GetInventoryItemsQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetInventoryItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await inventoryItemService.GetItemsAsync();
        if (items == null || items.Count <= 0)
            return await ResponseWrapper.FailAsync("Inventory Items does not exists.");

        return await ResponseWrapper<List<InventoryItemResponse>>.SuccessAsync(items.Adapt<List<InventoryItemResponse>>());
    }
}