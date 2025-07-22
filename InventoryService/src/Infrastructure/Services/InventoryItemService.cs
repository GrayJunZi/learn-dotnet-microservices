using Application.Features.InventoryItems;
using Domain;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using ResponseWrapperLibrary.Models.Responses.Products;
using ResponseWrapperLibrary.Wrappers;
using System.Net.Http;

namespace Infrastructure.Services;

public class InventoryItemService(
    ApplicationDbContext db,
    IHttpClientFactory httpClientFactory) : IInventoryItemService
{
    public async Task CreateItemAsync(List<InventoryItem> items)
    {
        await db.InventoryItems.AddRangeAsync(items);
        await db.SaveChangesAsync();
    }

    public async Task<InventoryItem> GetItemByIdAsync(int id)
    {
        return await db.InventoryItems.FindAsync(id);
    }

    public async Task<List<InventoryItem>> GetItemsByProductIdAsync(int productId)
    {
        return await db.InventoryItems.Where(x => x.ProductId == productId)
            .ToListAsync();
    }

    public async Task<List<InventoryItem>> GetItemsAsync()
    {
        return await db.InventoryItems.ToListAsync();
    }

    public async Task<int> ChangeItemsStatusAsync(List<InventoryItem> items)
    {
        db.InventoryItems.UpdateRange(items);
        return await db.SaveChangesAsync();
    }

    public async Task<int> DeleteItemAsync(int id)
    {
        var item = await db.InventoryItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item == null)
            return 0;

        db.InventoryItems.Remove(item);
        await db.SaveChangesAsync();
        return item.Id;
    }

    public async Task<ResponseWrapper<ProductResponse>> GetProductByIdAsync(int productId)
    {
        var client = httpClientFactory.CreateClient("ProductServiceClient");

        var response = await client.GetAsync($"/product/products/{productId}");

        return await response.ToResponse<ProductResponse>();
    }
}
