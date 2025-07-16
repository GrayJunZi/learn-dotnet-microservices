namespace ResponseWrapperLibrary.Models.Requests.Products.Events;

public class ProductCreatedEvent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ReOrderThreshHold { get; set; }
    public int Quantity { get; set; }
}
