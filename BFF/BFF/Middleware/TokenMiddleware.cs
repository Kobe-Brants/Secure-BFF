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

    public async Task InvokeAsync(HttpContext context, IGenericRepository<Session> sessionRepository)
    {
        if (context.Request.Path.StartsWithSegments("/api/authentication"))
        {
            await _next(context);
            return;
        }

        var sessionId = context.Request.Cookies["session_id"];
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

        context.Request.Headers.Authorization = $"Bearer {session.AccessToken}";
        await _next(context);
    }
}

public static class TokenMiddlewareExtensions
{
    public static void UseTokenMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<TokenMiddleware>();
    }
}