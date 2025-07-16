using Application.Features.Products;
using Domain;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

/// <summary>
/// 产品服务
/// </summary>
public class ProductService(ApplicationDbContext db) : IProductService
{
    /// <summary>
    /// 创建产品
    /// </summary>
    /// <param name="createProduct"></param>
    /// <returns></returns>
    public async Task<int> CreateProductAsync(Product createProduct)
    {
        await db.Products.AddAsync(createProduct);
        await db.SaveChangesAsync();
        return createProduct.Id;
    }
    /// <summary>
    /// 修改产品
    /// </summary>
    /// <param name="updateProduct"></param>
    /// <returns></returns>
    public async Task<Product> UpdateProductAsync(Product updateProduct)
    {
        db.Products.Update(updateProduct);
        await db.SaveChangesAsync();
        return updateProduct;
    }
    /// <summary>
    /// 删除产品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> DeleteProductAsync(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product == null)
            return 0;

        db.Products.Remove(product);
        await db.SaveChangesAsync();
        return product.Id;
    }
    /// <summary>
    /// 根据产品Id获取产品信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Product> GetByIdAsync(int id)
    {
        return await db.Products.FindAsync(id);
    }
    /// <summary>
    /// 获取所有产品信息
    /// </summary>
    /// <returns></returns>
    public async Task<List<Product>> GetAllAsync()
    {
        return await db.Products.ToListAsync();
    }

    /// <summary>
    /// 产品是否存在
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> IsExistsAsync(int id)
    {
        return await db.Products.AnyAsync(x => x.Id == id);
    }
}
