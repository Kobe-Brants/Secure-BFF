namespace BL.Services.Session
{
    public interface ISessionService
    {
        IEnumerable<Domain.Session> GetSessions(CancellationToken cancellationToken = default);

        Domain.Session? GetSession(Guid sessionId, CancellationToken cancellationToken);

        Task CreateSession(Domain.Session session, CancellationToken cancellationToken);

        Task ModifySession(Domain.Session session, CancellationToken cancellationToken);

        Task DeleteSession(Guid sessionId, CancellationToken cancellationToken);
    }
}
