using PracticeLogger.Application.Sessions.Commands;
using PracticeLogger.Domain.Models;
using PracticeLogger.Tests.Unit.Infrastructure;

namespace PracticeLogger.Tests.Unit.Sessions.Commands;

public class UpdateSessionCommandHandlerTests
{
    [Theory]
    [AutoFixtureData]
    public async Task Handle_WhenSessionExists_UpdatesSession(UpdateSessionCommand command)
    {
        using var db = new TestDbContext();
        var existing = new Session(0, DateOnly.FromDateTime(DateTime.UtcNow.Date), "Scales", 10, "Old");
        db.Context.PracticeSessions.Add(existing);
        await db.Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        db.Context.ChangeTracker.Clear();

        var handler = new UpdateSessionCommandHandler(db.Context);
        var updatedCommand = command with { Id = existing.Id };

        await handler.Handle(updatedCommand, TestContext.Current.CancellationToken);

        var saved = db.Context.PracticeSessions.Single(s => s.Id == existing.Id);
        Assert.Equal(updatedCommand.Activity, saved.Activity);
        Assert.Equal(updatedCommand.Minutes, saved.Minutes);
        Assert.Equal(updatedCommand.Date, saved.Date);
        Assert.Equal(updatedCommand.Notes, saved.Notes);
    }

    [Theory]
    [AutoFixtureData]
    public async Task Handle_WhenSessionMissing_DoesNotCreateSession(UpdateSessionCommand command)
    {
        using var db = new TestDbContext();
        var handler = new UpdateSessionCommandHandler(db.Context);

        await handler.Handle(command, TestContext.Current.CancellationToken);

        Assert.Empty(db.Context.PracticeSessions);
    }
}
