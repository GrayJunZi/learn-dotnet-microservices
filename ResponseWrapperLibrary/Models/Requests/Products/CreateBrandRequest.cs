namespace ResponseWrapperLibrary.Models.Requests.Products;

public class CreateBrandRequest
{
    /// <summary>
    /// 品牌名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 品牌描述
    /// </summary>
    public string Description { get; set; }
}
