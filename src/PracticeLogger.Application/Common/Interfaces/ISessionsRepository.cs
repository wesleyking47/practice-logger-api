using Models;

namespace Common.Interfaces;

public interface ISessionsRepository
{
    IEnumerable<Session> GetAllSessions();
}
