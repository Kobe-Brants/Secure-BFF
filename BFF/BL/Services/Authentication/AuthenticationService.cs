using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using BL.Services.Authentication.DTO_s.Responses;
using Core.Interfaces.Repositories;
using Domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BL.Services.Authentication;

public class AuthenticationService(IConfiguration configuration, IGenericRepository<Session> sessionRepository)
    : IAuthenticationService
{
    public string CreateSignInUrl(CancellationToken cancellationToken)
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
        sessionRepository.Add(session, cancellationToken);
        sessionRepository.Save(cancellationToken);

        return
            $"{session.AuthorizationEndpoint}?response_type=code&client_id={session.ClientId}&redirect_uri=https://localhost:7207/api/authentication/callback&scope={session.Scopes}&state={session.Id}&code_challenge={codeChallenge}&code_challenge_method=S256";
    }

    public async Task<string?> HandleCallback(string code, string state, CancellationToken cancellationToken)
    {
        var frontendRedirectUri = configuration.GetValue<string>("OAuth:FrontendRedirectUrl") ?? string.Empty;
        
        var existingSession = sessionRepository.Find(x => x.Id == state).FirstOrDefault();
        if (existingSession is null) return null;
        
        var tokenResponse = await ExchangeCodeForToken(code, existingSession.CodeVerifier);
        if (tokenResponse is null) return null;

        existingSession.AccessToken = tokenResponse.AccessToken;
        existingSession.RefreshToken = tokenResponse.RefreshToken;
        sessionRepository.Update(existingSession);
        await sessionRepository.Save(cancellationToken);

        return frontendRedirectUri;
    }

    public async Task<UserInfoResponse?> GetUserInfoOfSession(string sessionId, CancellationToken cancellationToken)
    {
        var client = new HttpClient();
        var userInfoEndpoint = configuration.GetValue<string>("OAuth:UserInfoEndpoint");
        var accessToken = sessionRepository.Find(x => x.Id == sessionId).FirstOrDefault()?.AccessToken;
        
        var request = new HttpRequestMessage(HttpMethod.Get, userInfoEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<UserInfoResponse>(content);
    }

    public async Task<bool> HandleSignOut(string sessionId, CancellationToken cancellationToken)
    {
        var session = sessionRepository.Find(x => x.Id == sessionId).FirstOrDefault();
        if (session is null) return false;
        
        sessionRepository.Delete(session);
        await sessionRepository.Save(cancellationToken);
        
        // TODO sign out sts
        return true;
    }

    private async Task<TokenResponses?> ExchangeCodeForToken(string code, string codeVerifier)
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
            { "client_secret", clientSecret },
            { "code_verifier", codeVerifier }
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

    // private async Task<bool> EndSession(Session session)
    // {
    //     var client = new HttpClient();
    //     var tokenEndpoint = configuration.GetValue<string>("OAuth:endSessionEndpoint");
    //     var postLogoutRedirectUri = configuration.GetValue<string>("OAuth:postLogoutRedirectUri");
    //         
    //     if (string.IsNullOrEmpty(session.IdToken))
    //     {
    //         throw new InvalidOperationException("Session does not contain a valid ID token.");
    //     }
    //
    //     var query = HttpUtility.ParseQueryString(string.Empty);
    //     query.Set("id_token_hint", session.IdToken);
    //     query.Set("post_logout_redirect_uri", postLogoutRedirectUri);
    //
    //     var state = Guid.NewGuid().ToString();
    //     query.Set("state", state);
    //
    //     var endSessionUrl = $"{tokenEndpoint}?{query}";
    //
    //     return true;
    // }
}