namespace Domain;

public class InventoryItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Barcode { get; set; }
    public string Status { get; set; }
    public DateTime CreatedOn { get; set; }
}
