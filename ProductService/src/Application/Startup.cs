using Application.Pipelines;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
            })
            .AddValidatorsFromAssembly(assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CachePipelineBehaviour<,>));
    }
}
