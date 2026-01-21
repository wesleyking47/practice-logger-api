using PracticeLogger.Application.Sessions.Queries.GetSessionsQuery;
using DomainSession = PracticeLogger.Domain.Models.Session;
using PracticeLogger.Tests.Unit.Infrastructure;

namespace PracticeLogger.Tests.Unit.Sessions.Queries;

public class GetSessionsQueryHandlerTests
{
    [Fact]
    public async Task Handle_FiltersByDateAndActivity()
    {
        using var db = new TestDbContext();
        var data = new[]
        {
            new DomainSession(0, new DateOnly(2024, 1, 10), "Scales", 10, null),
            new DomainSession(0, new DateOnly(2024, 1, 11), "Chords", 20, null),
            new DomainSession(0, new DateOnly(2024, 1, 12), "Scales", 30, null)
        };

        db.Context.PracticeSessions.AddRange(data);
        await db.Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var handler = new GetSessionsQueryHandler(db.Context);
        var query = new GetSessionsQuery(new DateOnly(2024, 1, 11), new DateOnly(2024, 1, 12), "Scales");

        var response = await handler.Handle(query, TestContext.Current.CancellationToken);

        var session = Assert.Single(response.Sessions);
        Assert.Equal("Scales", session.Activity);
        Assert.Equal(new DateOnly(2024, 1, 12), session.Date);
    }
}
