namespace ResponseWrapperLibrary.Models.Requests.Products;

public class UpdateBrandRequest
{
    /// <summary>
    /// 品牌Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 品牌名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 品牌描述
    /// </summary>
    public string Description { get; set; }
}
