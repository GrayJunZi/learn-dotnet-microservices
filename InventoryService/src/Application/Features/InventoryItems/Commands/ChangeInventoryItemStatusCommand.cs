using MediatR;
using ResponseWrapperLibrary.Models.Requests.Inventories;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.InventoryItems.Commands;

public class ChangeInventoryItemStatusCommand : IRequest<IResponseWrapper>
{
    public ChangeInventoryItemStatusRequest ChangeInventoryItemStatus { get; set; }
}

public class ChangeInventoryItemStatusCommandHandler(IInventoryItemService inventoryItemService) : IRequestHandler<ChangeInventoryItemStatusCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(ChangeInventoryItemStatusCommand request, CancellationToken cancellationToken)
    {
        var item = await inventoryItemService.GetItemByIdAsync(request.ChangeInventoryItemStatus.Id);
        if (item == null)
            return await ResponseWrapper.FailAsync("Inventory Item does not exists.");

        item.Status = request.ChangeInventoryItemStatus.Status;
        await inventoryItemService.ChangeItemsStatusAsync([item]);
        return await ResponseWrapper.SuccessAsync($"Inventory Item Status change to '{item.Status}'");
    }
}
