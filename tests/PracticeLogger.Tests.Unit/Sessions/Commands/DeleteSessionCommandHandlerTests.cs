using PracticeLogger.Application.Common.Interfaces;
using PracticeLogger.Application.Sessions.Commands;
using PracticeLogger.Domain.Models;
using PracticeLogger.Tests.Unit.Infrastructure;
using NSubstitute;

namespace PracticeLogger.Tests.Unit.Sessions.Commands;

public class DeleteSessionCommandHandlerTests
{
    [Fact]
    public async Task Handle_DeletesSession()
    {
        using var db = new TestDbContext();
        db.Context.Users.Add(new User { Id = 1, Username = "test", PasswordHash = "hash" });
        var session = new Session(0, DateOnly.FromDateTime(DateTime.UtcNow.Date), "Arpeggios", 15, null) { UserId = 1 };
        db.Context.PracticeSessions.Add(session);
        await db.Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var sessionId = db.Context.PracticeSessions.Single().Id;

        var mockUserService = Substitute.For<ICurrentUserService>();
        mockUserService.UserId.Returns(1);

        var handler = new DeleteSessionCommandHandler(db.Context, mockUserService);

        await handler.Handle(new DeleteSessionCommand(sessionId), TestContext.Current.CancellationToken);

        Assert.Empty(db.Context.PracticeSessions);
    }
}
