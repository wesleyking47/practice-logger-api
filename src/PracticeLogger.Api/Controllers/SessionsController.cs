using MediatR;
using Microsoft.AspNetCore.Mvc;
using PracticeLogger.Application.Sessions.Commands;
using PracticeLogger.Application.Sessions.Queries.GetSessionsQuery;

namespace PracticeLogger.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize]
public class SessionsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<ActionResult<int>> CreateSession(
        [FromBody] CreateSessionCommand createSessionCommand
    )
    {
        var response = await _sender.Send(createSessionCommand);

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
        var response = await _sender.Send(query);
        return response;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSession(int id)
    {
        var command = new DeleteSessionCommand(id);
        await _sender.Send(command);
        return Ok();
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateSession(int id, [FromBody] UpdateSessionCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await _sender.Send(command);

        return NoContent();
    }
}
