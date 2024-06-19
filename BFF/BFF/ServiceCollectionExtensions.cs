
using BL.Services.Authentication;
using BL.Services.Proxy;

namespace BFF;

public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IProxyService, ProxyService>();
    }
}