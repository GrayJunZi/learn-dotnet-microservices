﻿using Application;
using Application.Common.Services;
using Application.Features.Brands;
using Application.Features.Images;
using Application.Features.Products;
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
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IBrandService, BrandService>()
            .AddScoped<IImageService, ImageService>()
            .AddScoped<IEventPublisher, EventPublisher>();
    }

    public static IServiceCollection ConfigureRabbitMQService(this IServiceCollection services, RabbitMQSettings rabbitMQSettings)
    {
        services.AddMassTransit(configure =>
        {
            configure.UsingRabbitMq((context, factory) =>
            {
                factory.Host(rabbitMQSettings.Host, "/", host =>
                {
                    host.Username(rabbitMQSettings.UserName);
                    host.Password(rabbitMQSettings.Password);
                });
            });
        });

        return services;
    }
}
