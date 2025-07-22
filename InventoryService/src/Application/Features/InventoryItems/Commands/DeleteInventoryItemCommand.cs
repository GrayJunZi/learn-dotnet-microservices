using MediatR;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.InventoryItems.Commands;

public class DeleteInventoryItemCommand : IRequest<IResponseWrapper>
{
    public int ItemId { get; set; }
}

public class DeleteInventoryItemCommandHandler(IInventoryItemService inventoryItemService) : IRequestHandler<DeleteInventoryItemCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(DeleteInventoryItemCommand request, CancellationToken cancellationToken)
    {
       var removeItemId =  await inventoryItemService.DeleteItemAsync(request.ItemId);
        if (removeItemId == 0)
            return await ResponseWrapper.FailAsync("Inventory Item does not exists.");

        return await  ResponseWrapper.SuccessAsync("Inventory Item Deleted successfully.");
    }
}
