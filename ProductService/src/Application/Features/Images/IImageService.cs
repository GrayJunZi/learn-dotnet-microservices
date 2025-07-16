using Domain;

namespace Application.Features.Images;

/// <summary>
/// 图片服务
/// </summary>
public interface IImageService
{
    /// <summary>
    /// 创建图片
    /// </summary>
    /// <param name="createImage"></param>
    /// <returns></returns>
    Task<int> CreateImageAsync(Image createImage);
    /// <summary>
    /// 删除图片
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(int id);
    /// <summary>
    /// 根据图片Id获取图片信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Image> GetByIdAsync(int id);
    /// <summary>
    /// 根据产品Id获取图片信息
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    Task<List<Image>> GetByProductIdAsync(int productId);
}
