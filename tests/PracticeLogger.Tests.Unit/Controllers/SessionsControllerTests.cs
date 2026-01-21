using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PracticeLogger.Api.Controllers;
using PracticeLogger.Application.Sessions.Commands;
using PracticeLogger.Application.Sessions.Queries.GetSessionsQuery;
using ResponseSession = PracticeLogger.Application.Sessions.Queries.GetSessionsQuery.Session;
using PracticeLogger.Tests.Unit.Infrastructure;

namespace PracticeLogger.Tests.Unit.Controllers;

public class SessionsControllerTests
{
    [Theory]
    [AutoFixtureData]
    public async Task CreateSession_ReturnsCreatedResult(CreateSessionCommand command)
    {
        var mediator = Substitute.For<IMediator>();
        mediator
            .Send(Arg.Any<CreateSessionCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(123));

        var controller = new SessionsController(mediator);

        var result = await controller.CreateSession(command);

        var created = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(123, created.Value);
        Assert.Equal("/api/sessions/123", created.Location);
    }

    [Fact]
    public async Task GetSessions_ReturnsResponse()
    {
        var response = new GetSessionsResponse(new List<ResponseSession>());
        var mediator = Substitute.For<IMediator>();
        mediator
            .Send(Arg.Any<GetSessionsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(response));

        var controller = new SessionsController(mediator);

        var result = await controller.GetSessions(null, null, null);

        Assert.Same(response, result.Value);
    }

    [Theory]
    [AutoFixtureData]
    public async Task UpdateSession_WhenIdMismatch_ReturnsBadRequest(UpdateSessionCommand command)
    {
        var mediator = Substitute.For<IMediator>();
        var controller = new SessionsController(mediator);

        var result = await controller.UpdateSession(command.Id + 1, command);

        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [AutoFixtureData]
    public async Task UpdateSession_WhenIdMatches_ReturnsNoContent(UpdateSessionCommand command)
    {
        var mediator = Substitute.For<IMediator>();
        mediator
            .Send(Arg.Any<UpdateSessionCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var controller = new SessionsController(mediator);

        var result = await controller.UpdateSession(command.Id, command);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteSession_ReturnsOk()
    {
        var mediator = Substitute.For<IMediator>();
        mediator
            .Send(Arg.Any<DeleteSessionCommand>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var controller = new SessionsController(mediator);

        var result = await controller.DeleteSession(10);

        Assert.IsType<OkResult>(result);
    }
}
