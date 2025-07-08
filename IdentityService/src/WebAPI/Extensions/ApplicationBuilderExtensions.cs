using Infrastructure.Contexts;

namespace WebAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        internal static async Task<IApplicationBuilder> SeedDatabase(this IApplicationBuilder app)
        {
            await using var scope = app.ApplicationServices.CreateAsyncScope();
            var seeder = scope.ServiceProvider.GetRequiredService<ApplicationDbSeeder>();
            await seeder.SeedIdentityDatabaseAsync();
            return app;
        }
    }
}
