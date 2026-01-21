using MediatR;
using Microsoft.AspNetCore.Mvc;
using PracticeLogger.Application.Sessions.Commands;
using PracticeLogger.Application.Sessions.Queries.GetSessionsQuery;

namespace PracticeLogger.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SessionsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<int>> CreateSession(
        [FromBody] CreateSessionCommand createSessionCommand
    )
    {
        var response = await _mediator.Send(createSessionCommand);

        return Created($"/api/sessions/{response}", response);
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSession(int id)
    {
        var command = new DeleteSessionCommand(id);
        await _mediator.Send(command);
        return Ok();
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSession(int id, [FromBody] UpdateSessionCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await _mediator.Send(command);

        return NoContent();
    }
}
