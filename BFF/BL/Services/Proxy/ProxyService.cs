using System.Net;
using Core.Interfaces.Repositories;

namespace BL.Services.Proxy;

public class ProxyService(IGenericRepository<Domain.Session> sessionRepository) : IProxyService
{
    public async Task<HttpResponseMessage> ProxyRequestAsync(HttpRequestMessage request, string sessionId, CancellationToken cancellationToken)
    {
        var client = new HttpClient();
        var session = sessionRepository.Find(x => x.Id == sessionId).FirstOrDefault();

        if (session is null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session.AccessToken);

        var response = await client.SendAsync(request, cancellationToken);
        return response;
    }
}