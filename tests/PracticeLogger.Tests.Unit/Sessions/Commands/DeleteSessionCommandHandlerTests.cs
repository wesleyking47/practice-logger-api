using PracticeLogger.Application.Sessions.Commands;
using PracticeLogger.Domain.Models;
using PracticeLogger.Tests.Unit.Infrastructure;

namespace PracticeLogger.Tests.Unit.Sessions.Commands;

public class DeleteSessionCommandHandlerTests
{
    [Fact]
    public async Task Handle_DeletesSession()
    {
        using var db = new TestDbContext();
        db.Context.PracticeSessions.Add(new Session(0, DateOnly.FromDateTime(DateTime.UtcNow.Date), "Arpeggios", 15, null));
        await db.Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        var sessionId = db.Context.PracticeSessions.Single().Id;

        var handler = new DeleteSessionCommandHandler(db.Context);

        await handler.Handle(new DeleteSessionCommand(sessionId), TestContext.Current.CancellationToken);

        Assert.Empty(db.Context.PracticeSessions);
    }
}
