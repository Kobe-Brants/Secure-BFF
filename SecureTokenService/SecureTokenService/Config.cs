using Duende.IdentityServer.Models;

namespace SecureTokenService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
        };
    
    public static IEnumerable<ApiResource> ApiResources =>
        new[]
        {
            new ApiResource("api1")
            {
                Scopes = new List<string>{ "scope1", "scope2" },
                ApiSecrets = new List<Secret>{ new("supersecret".Sha256()) }
            }
        };

    public static IEnumerable<Client> Clients =>
        new[]
        {
            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "bff",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,

                RedirectUris = { "https://localhost:7207/api/authentication/callback" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2", "offline_access" },
                
                AccessTokenLifetime = 60 * 5,
                AbsoluteRefreshTokenLifetime =  60 * 30
            }
        };
}