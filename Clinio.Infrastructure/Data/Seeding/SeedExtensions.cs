using Microsoft.Extensions.DependencyInjection;

namespace Clinio.Infrastructure.Data.Seeding;

public static class SeedExtensions
{
    public static async Task SeedDatabaseAsync(
        this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var seeder = scope.ServiceProvider
            .GetRequiredService<IDatabaseSeeder>();

        await seeder.SeedAsync();
    }
}