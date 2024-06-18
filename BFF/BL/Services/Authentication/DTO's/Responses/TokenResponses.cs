using Newtonsoft.Json;

namespace BL.Services.Authentication.DTO_s.Responses;

public class TokenResponses
{ 
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }
        
        [JsonProperty("refresh_token")]
        public required string RefreshToken { get; set; }
}