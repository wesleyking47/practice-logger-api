using MediatR;
using Microsoft.EntityFrameworkCore;
using PracticeLogger.Application.Common.Interfaces;

namespace PracticeLogger.Application.Sessions.Queries.GetSessionsQuery;

public class GetSessionsQueryHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<GetSessionsQuery, GetSessionsResponse>
{
    private readonly IApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task<GetSessionsResponse> Handle(
        GetSessionsQuery query,
        CancellationToken cancellationToken
    )
    {
        var sessions = await _applicationDbContext
            .PracticeSessions.AsNoTracking()
            .Where(session =>
                (query.StartDate == null || session.Date >= query.StartDate)
                && (query.EndDate == null || session.Date <= query.EndDate)
                && (string.IsNullOrWhiteSpace(query.Activity) || session.Activity == query.Activity)
            )
            .Select(session => new Session(
                session.Id,
                session.Date,
                session.Activity,
                session.Minutes,
                session.Notes
            ))
            .ToListAsync(cancellationToken);

        return new GetSessionsResponse(sessions);
    }
}
