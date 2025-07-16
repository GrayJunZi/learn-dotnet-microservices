using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    /// <summary>
    /// 品牌
    /// </summary>
    public DbSet<Brand> Brands { get; set; }
    /// <summary>
    /// 产品
    /// </summary>
    public DbSet<Product> Products { get; set; }
    /// <summary>
    /// 图片
    /// </summary>
    public DbSet<Image> Images { get; set; }
}
