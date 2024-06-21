using Newtonsoft.Json;

namespace BL.Services.Authentication.DTO_s.Responses;

public class TokenResponses
{
    [JsonProperty("id_token")] 
    public required string IdToken { get; set; }

    [JsonProperty("access_token")] 
    public required string AccessToken { get; set; }

    [JsonProperty("refresh_token")] 
    public required string RefreshToken { get; set; }

    [JsonProperty("expires_in")] 
    public required int ExpiresIn { get; set; }
}