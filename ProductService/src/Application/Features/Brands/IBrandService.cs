using Domain;

namespace Application.Features.Brands;

/// <summary>
/// 品牌服务
/// </summary>
public interface IBrandService
{
    /// <summary>
    /// 创建品牌
    /// </summary>
    /// <param name="createBrand"></param>
    /// <returns></returns>
    Task<int> CreateBrandAsync(Brand createBrand);
    /// <summary>
    /// 修改品牌
    /// </summary>
    /// <param name="updateBrand"></param>
    /// <returns></returns>
    Task<Brand> UpdateBrandAsync(Brand updateBrand);
    /// <summary>
    /// 删除品牌
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<int> DeleteBrandAsync(int id);
    /// <summary>
    /// 根据品牌Id获取品牌信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Brand> GetBydIdAsync(int id);
    /// <summary>
    /// 获取所有品牌信息
    /// </summary>
    /// <returns></returns>
    Task<List<Brand>> GetAllAsync();

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> IsExistsAsync(int id);
}
