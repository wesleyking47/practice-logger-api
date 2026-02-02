using PracticeLogger.Application.Common.Interfaces;
using PracticeLogger.Application.Sessions.Commands;
using PracticeLogger.Tests.Unit.Infrastructure;
using NSubstitute;

namespace PracticeLogger.Tests.Unit.Sessions.Commands;

public class CreateSessionCommandHandlerTests
{
    [Theory]
    [AutoFixtureData]
    public async Task Handle_CreatesSessionAndReturnsId(CreateSessionCommand command)
    {
        using var db = new TestDbContext();
        db.Context.Users.Add(new Domain.Models.User { Id = 1, Username = "test", PasswordHash = "hash" });
        await db.Context.SaveChangesAsync(TestContext.Current.CancellationToken);
        
        var mockUserService = Substitute.For<ICurrentUserService>();
        mockUserService.UserId.Returns(1);

        var handler = new CreateSessionCommandHandler(db.Context, mockUserService);

        var id = await handler.Handle(command, TestContext.Current.CancellationToken);

        var saved = db.Context.PracticeSessions.Single();
        Assert.Equal(id, saved.Id);
        Assert.Equal(command.Activity, saved.Activity);
        Assert.Equal(command.Minutes, saved.Minutes);
        Assert.Equal(command.Date, saved.Date);
        Assert.Equal(command.Notes, saved.Notes);
        Assert.Equal(1, saved.UserId);
    }
}
