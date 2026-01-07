using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sessions.Queries.GetSessionsQuery;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class SessionsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("Sessions")]
    public Task<ActionResult> CreateSession()
    {
        return null;
    }

    [HttpGet("Sessions")]
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
