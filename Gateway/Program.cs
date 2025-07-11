using Gateway.Extensions;
using Gateway.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(path: "ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddOcelot();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddIdentitySettings();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("cors");

app.UseMiddleware<ErrorHandlingMiddleware>();

await app.UseOcelot();

app.Run();