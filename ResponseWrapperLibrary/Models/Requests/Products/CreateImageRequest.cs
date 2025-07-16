namespace ResponseWrapperLibrary.Models.Requests.Products;

public class CreateImageRequest
{
    /// <summary>
    /// 产品Id
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// 图片名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 图片Base64
    /// </summary>
    public string Data { get; set; }
}
