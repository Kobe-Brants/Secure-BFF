using Newtonsoft.Json;

namespace BL.Services.Session.DTO_s.Responses;

public class TokenResponses
{ 
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }
}