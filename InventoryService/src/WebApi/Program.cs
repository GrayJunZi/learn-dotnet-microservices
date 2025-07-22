using Application;
using Infrastructure;
using WebApi.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentitySettings();
builder.Services.AddJwtAuthentication(builder.Services.GetTokenSettings(builder.Configuration));
builder.Services.AddScalarDocumentation();

builder.Services.ConfigureRabbitMQService(builder.Services.GetRabbitMQSettings(builder.Configuration));
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddApplicationPackages();

builder.Services.RegisterNamedHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.Run();