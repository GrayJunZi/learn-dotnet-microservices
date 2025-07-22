using Application.Features.InventoryItems.Commands;
using MassTransit;
using MediatR;
using ResponseWrapperLibrary.Models.Requests.Inventories;
using ResponseWrapperLibrary.Models.Requests.Products.Events;

namespace Application.Features.InventoryItems.Consumers;

public class ProductCreatedEventConsumer(ISender sender) : IConsumer<ProductCreatedEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var productCreatedEventData = context.Message;

        var response = await sender.Send(new CreateInventoryItemCommand
        {
            CreateInventoryItems = generateCreateInventoryItems(productCreatedEventData),
        });
    }

    private static List<CreateInventoryItemRequest> generateCreateInventoryItems(ProductCreatedEvent productCreatedEvent)
    {
        return [.. Enumerable.Range(1, productCreatedEvent.Quantity).Select(x => new CreateInventoryItemRequest
        {
            ProductId = productCreatedEvent.Id,
            Status = "InStock",
            Barcode = generateUniqueBarcode(productCreatedEvent.Id)
        })];
    }

    private static string generateUniqueBarcode(int productId)
    {
        var uniquePart = $"{DateTime.Now:yyyyMMddHHmmssfff}-{Guid.NewGuid().ToString("N")[0..6].ToUpper()}";

        return $"{productId}-{uniquePart}";
    }
}
