using PracticeLogger.Application.Common.Interfaces;
using PracticeLogger.Application.Sessions.Queries.GetSessionsQuery;
using DomainSession = PracticeLogger.Domain.Models.Session;
using PracticeLogger.Tests.Unit.Infrastructure;
using NSubstitute;

namespace PracticeLogger.Tests.Unit.Sessions.Queries;

public class GetSessionsQueryHandlerTests
{
    [Fact]
    public async Task Handle_FiltersByDateAndActivity()
    {
        using var db = new TestDbContext();
        db.Context.Users.Add(new Domain.Models.User { Id = 1, Username = "test", PasswordHash = "hash" });

        var data = new[]
        {
            new DomainSession(0, new DateOnly(2024, 1, 10), "Scales", 10, null) { UserId = 1 },
            new DomainSession(0, new DateOnly(2024, 1, 11), "Chords", 20, null) { UserId = 1 },
            new DomainSession(0, new DateOnly(2024, 1, 12), "Scales", 30, null) { UserId = 1 }
        };

        db.Context.PracticeSessions.AddRange(data);
        await db.Context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var mockUserService = Substitute.For<ICurrentUserService>();
        mockUserService.UserId.Returns(1);

        var handler = new GetSessionsQueryHandler(db.Context, mockUserService);
        var query = new GetSessionsQuery(new DateOnly(2024, 1, 11), new DateOnly(2024, 1, 12), "Scales");

        var response = await handler.Handle(query, TestContext.Current.CancellationToken);

        var session = Assert.Single(response.Sessions);
        Assert.Equal("Scales", session.Activity);
        Assert.Equal(new DateOnly(2024, 1, 12), session.Date);
    }
}
