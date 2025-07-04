using ForkliftControlSystem.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ForkliftControlSystem.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task Initialize(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ForkliftDbContext>();

        // Apply pending migrations
        dbContext.Database.Migrate();

        // Seed data from JSON
        var forkliftService = scope.ServiceProvider.GetRequiredService<IForkliftService>();
        await forkliftService.SeedForkliftDataAsync();
    }
}
