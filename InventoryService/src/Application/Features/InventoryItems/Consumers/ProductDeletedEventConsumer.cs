using Application.Features.InventoryItems.Commands;
using MassTransit;
using MediatR;
using ResponseWrapperLibrary.Models.Requests.Products.Events;

namespace Application.Features.InventoryItems.Consumers;

public class ProductDeletedEventConsumer(ISender sender) : IConsumer<ProductDeletedEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        await sender.Send(new ChangeInventoryItemsStatusCommand
        {
            ChangeInventoryItemStatus = new ResponseWrapperLibrary.Models.Requests.Inventories.ChangeInventoryItemStatusRequest()
            {
                Id = context.Message.Id,
                Status = "Removed"
            },
            ProductId = context.Message.Id,
        });
    }
}
