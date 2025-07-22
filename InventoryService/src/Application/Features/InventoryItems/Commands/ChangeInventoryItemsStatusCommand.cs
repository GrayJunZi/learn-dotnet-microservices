using MediatR;
using ResponseWrapperLibrary.Models.Requests.Inventories;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.InventoryItems.Commands;

public class ChangeInventoryItemsStatusCommand : IRequest<IResponseWrapper>
{
    public ChangeInventoryItemStatusRequest ChangeInventoryItemStatus { get; set; }
    public int ProductId { get; set; }
}

public class ChangeInventoryItemsStatusCommandHandler(IInventoryItemService inventoryItemService) : IRequestHandler<ChangeInventoryItemsStatusCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(ChangeInventoryItemsStatusCommand request, CancellationToken cancellationToken)
    {
        var inventoryItems = await inventoryItemService.GetItemsByProductIdAsync(request.ChangeInventoryItemStatus.Id);
        if (inventoryItems == null || inventoryItems.Count <= 0)
            return await ResponseWrapper.FailAsync($"No Inventory Items were found for product Id:{request.ProductId}");

        foreach (var item in inventoryItems)
        {
            item.Status = request.ChangeInventoryItemStatus.Status;
        }
        await inventoryItemService.ChangeItemsStatusAsync(inventoryItems);

        return await ResponseWrapper.SuccessAsync("Inventory Items marked removed successfully.");
    }
}