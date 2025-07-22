using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Application;

public static class Startup
{
    public static IServiceCollection AddApplicationPackages(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        return services
            .AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblies(assembly);
            });
    }
}
