using ForkliftControlSystem.Application.DTOs;

namespace ForkliftControlSystem.Application.Interfaces;

public interface ICommandService
{
    List<string> ParseCommand(string command);
    CommandResult ExecuteCommand(string command);
    CommandResult ExecuteCommandWithObstacles(string command, List<ObstacleDto> obstacles);
}
