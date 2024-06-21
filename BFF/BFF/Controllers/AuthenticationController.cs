using BL.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpGet("sign-in")]
    public IActionResult SignIn(CancellationToken cancellationToken)
    {
        var authorizationUrl = authenticationService.CreateSignInUrl(cancellationToken);
        return Ok(authorizationUrl);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code, string state, CancellationToken cancellationToken)
    {
        var frontendRedirectUri = await authenticationService.HandleCallback(code, state, cancellationToken);

        if (frontendRedirectUri is null) return Ok();
        var secureCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(0.5)
        };
    
        Response.Cookies.Append("__Host-session-id", state, secureCookieOptions);
        return Redirect(frontendRedirectUri);
    }
    
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var sessionId = Request.Cookies["__Host-session-id"];
        if (sessionId is null) return Unauthorized();

        var userInfo = await authenticationService.GetUserInfoOfSession(sessionId, cancellationToken);
        return Ok(userInfo);
    }
    
    
    [HttpGet("sign-out")]
    public async Task<IActionResult> SignOut(CancellationToken cancellationToken)
    {
        var sessionId = Request.Cookies["__Host-session-id"];
        if (sessionId is null) return Unauthorized();

        var isSignedOut = await authenticationService.HandleSignOut(sessionId, cancellationToken);

        if (!isSignedOut) return StatusCode(StatusCodes.Status500InternalServerError, "Failed to sign out");

        Response.Cookies.Delete("__Host-session-id");
        Response.Cookies.Delete("__Host-session-user");
        return Ok();
    }
}