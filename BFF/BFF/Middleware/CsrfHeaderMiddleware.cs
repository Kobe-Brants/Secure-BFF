namespace BFF.Middleware;

public class CsrfHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private const string CsrfTokenHeader = "x-csrf-header";
    private readonly string[] _excludedPaths = ["/api/authentication/callback"];

    public CsrfHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_excludedPaths.Contains(context.Request.Path.ToString()))
        {
            if (!context.Request.Headers.ContainsKey(CsrfTokenHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("CSRF token is missing.");
                return;
            }
        }

        await _next(context);
    }
}