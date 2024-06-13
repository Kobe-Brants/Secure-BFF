namespace Domain;

public class Session()
{
    public required string Id { get; set; }
    public required string ClientId { get; set; }
    public required string AuthorizationEndpoint { get; set; }
    public required string Scopes { get; set; }
    public required string CodeVerifier { get; set; }
}