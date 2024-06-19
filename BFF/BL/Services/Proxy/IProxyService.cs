namespace BL.Services.Proxy;

public interface IProxyService
{
    Task<HttpResponseMessage> ProxyRequestAsync(HttpRequestMessage request, string sessionId, CancellationToken cancellationToken);
}