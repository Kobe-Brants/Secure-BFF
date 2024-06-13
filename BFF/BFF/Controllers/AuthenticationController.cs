using BFF.Extensions;
using BL.Services.Session;
using BL.Services.Session.DTO_s.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BFF.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthenticationController: ControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly IConfiguration _configuration;

    public AuthenticationController(ISessionService sessionService, IConfiguration configuration)
    {
        _sessionService = sessionService;
        _configuration = configuration;
    }
    
    
    [HttpGet("signin")]
    public IActionResult SignIn()
    {
        var authorizationUrl = GenerateAuthorizationUrl();
        return Redirect(authorizationUrl);
    }
    
    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        var tokenResponse = await ExchangeCodeForToken(code);
        
        // TODO
        return Ok();
    }
    
    private string GenerateAuthorizationUrl()
    {
        var clientId = _configuration.GetValue<string>("OAuth:ClientId");
        var redirectUri = _configuration.GetValue<string>("OAuth:RedirectUri");
        var authorizationEndpoint = _configuration.GetValue<string>("OAuth:AuthorizationEndpoint");
        var state = GenerateState();
        
        return $"{authorizationEndpoint}?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&state={state}";
    }

    private async Task<TokenResponses?> ExchangeCodeForToken(string code)
    {
        var client = new HttpClient();
        var tokenEndpoint = _configuration.GetValue<string>("OAuth:TokenEndpoint");
        var clientId = _configuration.GetValue<string>("OAuth:ClientId");
        var clientSecret = _configuration.GetValue<string>("OAuth:ClientSecret");
        var redirectUri = _configuration.GetValue<string>("OAuth:RedirectUri");

        if (clientId is null || redirectUri is null || clientSecret is null)
        {
            throw new Exception("Configuration value not found");
        }

        var requestBody = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", redirectUri },
            { "client_id", clientId },
            { "client_secret", clientSecret.Sha256() }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
        {
            Content = new FormUrlEncodedContent(requestBody)
        };

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TokenResponses>(content);
    }
    
    private static string GenerateState()
    {
        return Guid.NewGuid().ToString();
    }
}