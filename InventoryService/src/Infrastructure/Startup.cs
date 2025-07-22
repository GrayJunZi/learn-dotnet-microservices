using Application;
using Application.Features.InventoryItems;
using Application.Features.InventoryItems.Consumers;
using Infrastructure.Contexts;
using Infrastructure.Services;
using MassTransit;
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
                }));
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IInventoryItemService, InventoryItemService>();
    }


    public static IServiceCollection ConfigureRabbitMQService(this IServiceCollection services, RabbitMQSettings rabbitMQSettings)
    {
        services.AddMassTransit(configure =>
        {
            configure.AddConsumer<ProductCreatedEventConsumer>();
            configure.AddConsumer<ProductDeletedEventConsumer>();
            configure.UsingRabbitMq((context, factory) =>
            {
                factory.Host(rabbitMQSettings.Host, "/", host =>
                {
                    host.Username(rabbitMQSettings.UserName);
                    host.Password(rabbitMQSettings.Password);
                });

                factory.ReceiveEndpoint(rabbitMQSettings.ProductCreatedEventQueue, ep =>
                {
                    ep.PrefetchCount = 10;
                    ep.UseMessageRetry(r => r.Interval(3, TimeSpan.FromMicroseconds(rabbitMQSettings.RetryInMilliseconds)));
                    ep.ConfigureConsumer<ProductCreatedEventConsumer>(context);
                });

                factory.ReceiveEndpoint(rabbitMQSettings.ProductDeletedEventQueue, ep =>
                {
                    ep.PrefetchCount = 10;
                    ep.UseMessageRetry(r => r.Interval(3, TimeSpan.FromMicroseconds(rabbitMQSettings.RetryInMilliseconds)));
                    ep.ConfigureConsumer<ProductDeletedEventConsumer>(context);
                });
            });
        });

        return services;
    }
}
