using Domain;

namespace Application.Features.Products;

/// <summary>
/// 产品服务
/// </summary>
public interface IProductService
{
    /// <summary>
    /// 创建产品
    /// </summary>
    /// <param name="createProduct"></param>
    /// <returns></returns>
    Task<int> CreateProductAsync(Product createProduct);
    /// <summary>
    /// 修改产品
    /// </summary>
    /// <param name="updateProduct"></param>
    /// <returns></returns>
    Task<Product> UpdateProductAsync(Product updateProduct);
    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<int> DeleteProductAsync(int id);
    /// <summary>
    /// 根据产品Id获取产品信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Product> GetByIdAsync(int id);
    /// <summary>
    /// 获取所有产品信息
    /// </summary>
    /// <returns></returns>
    Task<List<Product>> GetAllAsync();
    /// <summary>
    /// 产品是否存在
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> IsExistsAsync(int id);
}
