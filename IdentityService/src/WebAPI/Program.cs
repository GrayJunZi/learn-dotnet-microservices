using Application;
using Infrastructure;
using Scalar.AspNetCore;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentitySettings();
builder.Services.AddIdentityServices();
builder.Services.AddApplicationServices();
builder.Services.AddJwtAuthentication(builder.Services.GetTokenSettings(builder.Configuration));
builder.Services.AddScalarDocumentation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await app.SeedDatabase();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();