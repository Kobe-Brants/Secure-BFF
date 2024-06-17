using Core.Interfaces.Repositories;
using Dal.DbContexts;
using Dal.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dal;

public static class ServiceCollectionExtensions
{
    public static void RegisterDbContexts(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<DatabaseContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly("BFF")));
    }

    public static void RegisterRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IGenericRepository<Session>, GenericRepository<Session, DatabaseContext>>();
    }
}