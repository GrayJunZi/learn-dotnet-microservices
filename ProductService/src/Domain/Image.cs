namespace Domain;

/// <summary>
/// 图片
/// </summary>
public class Image
{
    /// <summary>
    /// 图片Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 图片名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 图片Base64
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// 产品Id
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// 产品
    /// </summary>
    public Product Product { get; set; }
}
