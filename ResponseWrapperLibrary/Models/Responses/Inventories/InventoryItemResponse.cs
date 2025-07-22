using ResponseWrapperLibrary.Models.Responses.Products;

namespace ResponseWrapperLibrary.Models.Responses.Inventories;

public class InventoryItemResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Barcode { get; set; }
    public string Status { get; set; }
    public DateTime CreatedOn { get; set; }

    public ProductResponse Product { get; set; }
}
