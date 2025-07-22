using Domain;
using ResponseWrapperLibrary.Models.Responses.Products;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.InventoryItems;

public interface IInventoryItemService
{
    Task CreateItemAsync(List<InventoryItem> items);
    Task<InventoryItem> GetItemByIdAsync(int id);
    Task<List<InventoryItem>> GetItemsByProductIdAsync(int productId);
    Task<List<InventoryItem>> GetItemsAsync();
    Task<int> ChangeItemsStatusAsync(List<InventoryItem> items);
    Task<int> DeleteItemAsync(int id);

    Task<ResponseWrapper<ProductResponse>> GetProductByIdAsync(int productId);
}
