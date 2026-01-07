using MediatR;

namespace PracticeLogger.Application.Sessions.Queries.GetSessionsQuery;

public record GetSessionsQuery(DateOnly? StartDate, DateOnly? EndDate, string? Activity)
    : IRequest<GetSessionsResponse>;
