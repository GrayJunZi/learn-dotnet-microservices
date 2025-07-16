namespace ResponseWrapperLibrary.Models.Requests.Products;

public class UpdateProductRequest
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 品牌Id
    /// </summary>
    public int BrandId { get; set; }
    /// <summary>
    /// 产品名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 产品描述
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// 重新订购阈值
    /// </summary>
    public int ReOrderThreshHold { get; set; }

    /// <summary>
    /// 库存
    /// </summary>
    public int Quantity { get; set; }
}
