using Dal.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BFF;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        context.Database.Migrate();
    }
}