using Application.Features.Roles;
using Application.Features.Token;
using Application.Features.Users;
using Infrastructure.Contexts;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"), builder =>
                {
                    builder.MigrationsHistoryTable("Migrations", "EFCore");
                    builder.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(100), errorNumbersToAdd: [1]);
                }))
            .AddTransient<ApplicationDbSeeder>();
    }

    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IUserService, UserService>()
            .AddTransient<IRoleService, RoleService>()
            .AddTransient<ITokenService, TokenService>();
    }
}
