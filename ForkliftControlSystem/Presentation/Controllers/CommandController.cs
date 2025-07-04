using ForkliftControlSystem.Application.DTOs;
using ForkliftControlSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftControlSystem.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ICommandService _commandService;

    public CommandsController(ICommandService commandService)
    {
        _commandService = commandService;
    }

    [HttpPost("track")]
    public IActionResult TrackCommand([FromBody] CommandRequest request)
    {
        try
        {
            var result = _commandService.ExecuteCommand(request.Command);
            return Ok(ApiResponse<CommandResult>.Ok(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpPost("parse")]
    public IActionResult ParseCommand([FromBody] CommandRequest request)
    {
        try
        {
            var actions = _commandService.ParseCommand(request.Command);
            return Ok(ApiResponse<List<string>>.Ok(actions));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpPost("trackwithobstacles")]
    public IActionResult TrackWithObstacles([FromBody] ObstacleCommandRequest request)
    {
        try
        {
            var result = _commandService.ExecuteCommandWithObstacles(request.Command, request.Obstacles ?? new());
            return Ok(ApiResponse<CommandResult>.Ok(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

}
