using Core.Interfaces.Repositories;

namespace BL.Services.Session;

public class SessionService : ISessionService
{
    private readonly IGenericRepository<Domain.Session> _sessionRepository;

    public SessionService(IGenericRepository<Domain.Session> sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public IEnumerable<Domain.Session> GetSessions(CancellationToken cancellationToken = default)
    {
        return _sessionRepository.GetAll();
    }

    public Domain.Session? GetSession(Guid sessionId, CancellationToken cancellationToken)
    {
        return _sessionRepository.Find(x => x.Id == sessionId).FirstOrDefault();
    }

    public async Task CreateSession(Domain.Session user, CancellationToken cancellationToken)
    {
        await _sessionRepository.Add(user, cancellationToken);
        await _sessionRepository.Save(cancellationToken);
    }

    public async Task ModifySession(Domain.Session session, CancellationToken cancellationToken)
    {
        var existingSession = _sessionRepository.Find(x => x.Id == session.Id).FirstOrDefault();

        if (existingSession is not null)
        {
            existingSession = session;
            _sessionRepository.Update(existingSession);
            await _sessionRepository.Save(cancellationToken);
        }
    }

    public async Task DeleteSession(Guid sessionId, CancellationToken cancellationToken)
    {
        var session = _sessionRepository.Find(x => x.Id == sessionId).FirstOrDefault();

        if (session is not null)
            _sessionRepository.Delete(session);
        
        await _sessionRepository.Save(cancellationToken);
    }
}