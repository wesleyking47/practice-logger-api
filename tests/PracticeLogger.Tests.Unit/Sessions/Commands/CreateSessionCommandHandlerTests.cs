using PracticeLogger.Application.Sessions.Commands;
using PracticeLogger.Tests.Unit.Infrastructure;

namespace PracticeLogger.Tests.Unit.Sessions.Commands;

public class CreateSessionCommandHandlerTests
{
    [Theory]
    [AutoFixtureData]
    public async Task Handle_CreatesSessionAndReturnsId(CreateSessionCommand command)
    {
        using var db = new TestDbContext();
        var handler = new CreateSessionCommandHandler(db.Context);

        var id = await handler.Handle(command, TestContext.Current.CancellationToken);

        var saved = db.Context.PracticeSessions.Single();
        Assert.Equal(id, saved.Id);
        Assert.Equal(command.Activity, saved.Activity);
        Assert.Equal(command.Minutes, saved.Minutes);
        Assert.Equal(command.Date, saved.Date);
        Assert.Equal(command.Notes, saved.Notes);
    }
}
