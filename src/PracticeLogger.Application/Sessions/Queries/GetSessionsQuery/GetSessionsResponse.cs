namespace Sessions.Queries.GetSessionsQuery;

public record GetSessionsResponse(IEnumerable<Session> Sessions);

public record Session(int Id, DateOnly Date, string Activity, int Minutes, string Notes);
