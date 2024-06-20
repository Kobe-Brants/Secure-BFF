using BL.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        var userInfo = await authenticationService.GetUserInfoOfSession(state, cancellationToken);
        
        var secureCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        };
        var vulnerableCookieOptions = new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        };
    
        Response.Cookies.Append("session_id", state, secureCookieOptions);
        Response.Cookies.Append("session_user", JsonConvert.SerializeObject(userInfo), vulnerableCookieOptions);

        return Redirect(frontendRedirectUri);
    }
    
    
    [HttpGet("sign-out")]
    public async Task<IActionResult> SignOut(CancellationToken cancellationToken)
    {
        var sessionId = Request.Cookies["session_id"];
        if (sessionId is null) return Unauthorized();

        var isSignedOut = await authenticationService.HandleSignOut(sessionId, cancellationToken);

        if (!isSignedOut) return StatusCode(StatusCodes.Status500InternalServerError, "Failed to sign out");

        Response.Cookies.Delete("session_id");
        Response.Cookies.Delete("session_user");
        return Ok();
    }
}