using BL.Services.Authentication;
using Core.Interfaces.Repositories;
using Domain;

namespace BFF.Middleware;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IGenericRepository<Session> sessionRepository, IAuthenticationService authenticationService)
    {
        // TODO isn't there a cleaner way?
        if (context.Request.Path.StartsWithSegments("/api/authentication"))
        {
            await _next(context);
            return;
        }

        var sessionId = context.Request.Cookies["__Host-session-id"];
        if (sessionId is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: No session ID found.");
            return;
        }

        var session = sessionRepository.Find(x => x.Id == sessionId).FirstOrDefault();
        if (session is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: No session ID found.");
            return;
        }

        if (!IsTokenValid(session.ExpiresAt ?? DateTime.Now))
        {
            var tokensSuccessfullyRefreshed = await authenticationService.HandleRefreshAccessToken(session.Id, CancellationToken.None);
            if (!tokensSuccessfullyRefreshed)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: No session ID found.");
            }
        }

        context.Request.Headers.Authorization = $"Bearer {session.AccessToken}";
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Service Unavailable: Authorization failed.");
        }    
    }
    
    private static bool IsTokenValid(DateTime tokenExpirationTime)
    {
        var currentTimeWithMargin = DateTime.UtcNow.AddMinutes(1);

        return tokenExpirationTime > currentTimeWithMargin;
    }
}

public static class TokenMiddlewareExtensions
{
    public static void UseTokenMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<TokenMiddleware>();
    }
}