using Clinio.Api;
using Clinio.Application;
using Clinio.Infrastructure;
using Clinio.Infrastructure.Data.Seeding;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseRequestLocalization(
    app.Services
        .GetRequiredService<IOptions<RequestLocalizationOptions>>()
        .Value);

if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDatabaseAsync();
    app.MapOpenApi();
    app.MapScalarApiReference();
    
    app.Use(async (context, next) =>
    {
        if (context.Request.Path.StartsWithSegments("/api/doctors"))
        {
            // await Task.Delay(2000);
        }
        await next();
    });
}

app.UseCors("DevPolicy");
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();

app.Run();