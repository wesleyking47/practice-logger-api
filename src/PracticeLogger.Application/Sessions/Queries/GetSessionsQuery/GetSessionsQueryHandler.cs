using Common.Interfaces;
using MediatR;

namespace Sessions.Queries.GetSessionsQuery;

public class GetSessionsQueryHandler(ISessionsRepository sessionRepository)
    : IRequestHandler<GetSessionsQuery, GetSessionsResponse>
{
    private readonly ISessionsRepository _sessionRepository = sessionRepository;

    public async Task<GetSessionsResponse> Handle(
        GetSessionsQuery query,
        CancellationToken cancellationToken
    )
    {
        var sessions = _sessionRepository
            .GetAllSessions()
            .Where(session =>
                (query.StartDate == null || session.Date >= query.StartDate)
                && (query.EndDate == null || session.Date <= query.EndDate)
                && (
                    string.IsNullOrWhiteSpace(query.Activity)
                    || string.Compare(
                        session.Activity,
                        query.Activity,
                        StringComparison.OrdinalIgnoreCase
                    ) == 0
                )
            )
            .Select(session => new Session(
                session.Id,
                session.Date,
                session.Activity,
                session.Minutes,
                session.notes
            ));

        return new GetSessionsResponse(sessions);
    }
}
