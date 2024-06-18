using Newtonsoft.Json;

namespace BL.Services.Authentication.DTO_s.Responses;

public class UserInfoResponse
{
    [JsonProperty("sub")]
    public required string Sub { get; set; }
    
    [JsonProperty("name")] 
    public required string Name { get; set; }
    
    [JsonProperty("given_name")] 
    public required string GivenName { get; set; }
    
    [JsonProperty("family_name")] 
    public required string FamilyName { get; set; }
    
    [JsonProperty("preferred_username")] 
    public required string PreferredUsername { get; set; }
}