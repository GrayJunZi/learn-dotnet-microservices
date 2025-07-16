using Application.Features.Images;
using Domain;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

/// <summary>
/// 图片服务
/// </summary>
public class ImageService(ApplicationDbContext db) : IImageService
{
    /// <summary>
    /// 创建图片
    /// </summary>
    /// <param name="createImage"></param>
    /// <returns></returns>
    public async Task<int> CreateImageAsync(Image createImage)
    {
        await db.Images.AddAsync(createImage);
        await db.SaveChangesAsync();
        return createImage.Id;
    }
    /// <summary>
    /// 删除图片
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(int id)
    {
        var image = await db.Images.FindAsync(id);
        if (image == null)
            return 0;

        db.Images.Remove(image);
        await db.SaveChangesAsync();
        return id;
    }
    /// <summary>
    /// 根据图片Id获取图片信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Image> GetByIdAsync(int id)
    {
        return await db.Images.FindAsync(id);
    }
    /// <summary>
    /// 根据产品Id获取图片信息
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public async Task<List<Image>> GetByProductIdAsync(int productId)
    {
        return await db.Images
            .Where(x => x.ProductId == productId)
            .ToListAsync();
    }
}
