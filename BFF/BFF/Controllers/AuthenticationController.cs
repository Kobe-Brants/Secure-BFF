using System.Security.Cryptography;
using System.Text;
using BFF.Extensions;
using BL.Services.Session;
using BL.Services.Session.DTO_s.Responses;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BFF.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(ISessionService sessionService, IConfiguration configuration)
    : ControllerBase
{

    [HttpGet("sign-in")]
    public async Task<IActionResult> SignIn(CancellationToken cancellationToken)
    {
        var session = new Session
        {
            Id = Guid.NewGuid().ToString(),
            ClientId = configuration.GetValue<string>("OAuth:ClientId") ?? string.Empty,
            AuthorizationEndpoint = configuration.GetValue<string>("OAuth:AuthorizationEndpoint") ?? string.Empty,
            Scopes = configuration.GetValue<string>("OAuth:Scopes") ?? string.Empty,
            CodeVerifier = GenerateCodeVerifier()
        };

        var codeChallenge = GenerateCodeChallenge(session.CodeVerifier);
        await sessionService.CreateSession(session, cancellationToken);

        var authorizationUrl = $"{session.AuthorizationEndpoint}?response_type=code&client_id={session.ClientId}&redirect_uri=https://localhost:7207/api/authentication/callback&scope={session.Scopes}&state={session.Id}&code_challenge={codeChallenge}&code_challenge_method=S256";
        return Redirect(authorizationUrl);
    }
    
    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        var tokenResponse = await ExchangeCodeForToken(code);
        
        // TODO
        return Ok();
    }

    private async Task<TokenResponses?> ExchangeCodeForToken(string code)
    {
        var client = new HttpClient();
        var tokenEndpoint = configuration.GetValue<string>("OAuth:TokenEndpoint");
        var clientId = configuration.GetValue<string>("OAuth:ClientId");
        var clientSecret = configuration.GetValue<string>("OAuth:ClientSecret");
        var redirectUri = configuration.GetValue<string>("OAuth:RedirectUri");

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
    
    private static string GenerateCodeVerifier()
    {
        var bytes = new byte[32];
        RandomNumberGenerator.Fill(bytes);
        return Base64UrlEncode(bytes);
    }

    private static string GenerateCodeChallenge(string codeVerifier)
    {
        var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));
        return Base64UrlEncode(challengeBytes);
    }

    private static string Base64UrlEncode(byte[] input)
    {
        var output = Convert.ToBase64String(input)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
        return output;
    }
}