using BL.Services.Session;

namespace BFF;

public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ISessionService, SessionService>();
    }
}