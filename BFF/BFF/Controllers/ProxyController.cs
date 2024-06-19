using BL.Services.Proxy;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Controllers;

[ApiController]
[Route("[controller]/{*url}")]
public class ProxyController(IProxyService proxyService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(string url, CancellationToken cancellationToken)
    {
        var sessionId = Request.Cookies["session_id"];
        if (sessionId is null) return Unauthorized();

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await proxyService.ProxyRequestAsync(requestMessage, sessionId, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return Content(content, response.Content.Headers.ContentType?.MediaType ?? string.Empty);
    }

    [HttpPost]
    public async Task<IActionResult> Post(string url, [FromBody] object body, CancellationToken cancellationToken)
    {
        var sessionId = Request.Cookies["session_id"];
        if (sessionId is null) return Unauthorized();
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(body.ToString() ?? string.Empty, System.Text.Encoding.UTF8, "application/json")
        };

        var response = await proxyService.ProxyRequestAsync(requestMessage, sessionId, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return Content(content, response.Content.Headers.ContentType?.MediaType ?? string.Empty);
    }

    // TODO Implement other HTTP methods as needed
}