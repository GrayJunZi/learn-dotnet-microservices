using Domain;
using Mapster;
using MediatR;
using ResponseWrapperLibrary.Models.Requests.Inventories;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.InventoryItems.Commands;

public class CreateInventoryItemCommand : IRequest<IResponseWrapper>
{
    public List<CreateInventoryItemRequest> CreateInventoryItems { get; set; }
}

public class CreateInventoryItemCommandHandler(IInventoryItemService inventoryItemService) : IRequestHandler<CreateInventoryItemCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var items = request.CreateInventoryItems.Adapt<List<InventoryItem>>();
        foreach (var item in items)
        {
            item.CreatedOn = now;
        }
        await inventoryItemService.CreateItemAsync(items);

        return await ResponseWrapper.SuccessAsync("Inventory Items created successfully.");
    }
}
