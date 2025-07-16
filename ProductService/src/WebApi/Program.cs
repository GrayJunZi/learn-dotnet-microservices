using Application;
using Infrastructure;
using WebApi.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddIdentitySettings();
builder.Services.AddJwtAuthentication(builder.Services.GetTokenSettings(builder.Configuration));

var cacheSettings = builder.Services.GetCacheSettings(builder.Configuration);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = cacheSettings.ConnectionString;
});

builder.Services.ConfigureRabbitMQService(builder.Configuration);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddApplicationPackages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
