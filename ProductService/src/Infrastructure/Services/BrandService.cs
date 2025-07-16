using Application.Features.Brands;
using Domain;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

/// <summary>
/// 品牌服务
/// </summary>
public class BrandService(ApplicationDbContext db) : IBrandService
{
    /// <summary>
    /// 创建品牌
    /// </summary>
    /// <param name="createBrand"></param>
    /// <returns></returns>
    public async Task<int> CreateBrandAsync(Brand createBrand)
    {
        await db.Brands.AddAsync(createBrand);
        await db.SaveChangesAsync();
        return createBrand.Id;
    }
    /// <summary>
    /// 修改品牌
    /// </summary>
    /// <param name="updateBrand"></param>
    /// <returns></returns>
    public async Task<Brand> UpdateBrandAsync(Brand updateBrand)
    {
        var brand = await db.Brands.FindAsync(updateBrand.Id);
        if (brand == null)
            return null;

        brand.Name = updateBrand.Name;
        brand.Description = updateBrand.Description;

        db.Brands.Update(brand);
        await db.SaveChangesAsync();
        return brand;
    }
    /// <summary>
    /// 删除品牌
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> DeleteBrandAsync(int id)
    {
        var brand = await db.Brands.FindAsync(id);
        if (brand == null)
            return 0;

        db.Brands.Remove(brand);
        await db.SaveChangesAsync();
        return id;
    }
    /// <summary>
    /// 根据品牌Id获取品牌信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Brand> GetBydIdAsync(int id)
    {
        return await db.Brands.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// 获取所有品牌信息
    /// </summary>
    /// <returns></returns>
    public async Task<List<Brand>> GetAllAsync()
    {
        return await db.Brands.ToListAsync();
    }


    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> IsExistsAsync(int id)
    {
        return await db.Brands.AnyAsync(x => x.Id == id);
    }
}
