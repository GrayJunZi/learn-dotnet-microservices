namespace ResponseWrapperLibrary.Models.Requests.Inventories;

public class ChangeInventoryItemStatusRequest
{
    public int Id { get; set; }
    public string Status { get; set; }
}
