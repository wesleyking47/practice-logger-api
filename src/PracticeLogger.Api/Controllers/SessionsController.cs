using MediatR;
using Microsoft.AspNetCore.Mvc;
using PracticeLogger.Application.Sessions.Queries.GetSessionsQuery;

namespace PracticeLogger.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SessionsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public Task<ActionResult> CreateSession()
    {
        return null;
    }

    [HttpGet]
    public async Task<ActionResult<GetSessionsResponse>> GetSessions(
        DateOnly? startDate,
        DateOnly? endDate,
        string? activity
    )
    {
        var query = new GetSessionsQuery(startDate, endDate, activity);
        var response = await _mediator.Send(query);
        return response;
    }
}
