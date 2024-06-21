using BL.Services.Authentication.DTO_s.Responses;

namespace BL.Services.Authentication;

public interface IAuthenticationService
{
    string CreateSignInUrl(CancellationToken cancellationToken);

    Task<string?> HandleCallback(string code, string state, CancellationToken cancellationToken);

    Task<UserInfoResponse?> GetUserInfoOfSession(string sessionId, CancellationToken cancellationToken);
    
    Task<bool> HandleSignOut(string sessionId, CancellationToken cancellationToken);

    Task<bool> HandleRefreshAccessToken(string sessionId, CancellationToken cancellationToken);
}