using MediatR;
using Microsoft.AspNetCore.Mvc;
using PracticeLogger.Application.Users.Commands.RegisterUser;
using PracticeLogger.Application.Users.Queries.LoginUser;

namespace PracticeLogger.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<ActionResult<int>> Register(RegisterUserCommand command)
    {
        var userId = await _sender.Send(command);
        return Ok(userId);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUserQuery query)
    {
        try
        {
            var token = await _sender.Send(query);
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
}
