using ForkliftControlSystem.Application.DTOs;
using ForkliftControlSystem.Application.Interfaces;
using ForkliftControlSystem.Domain.Enums;
using ForkliftControlSystem.Domain.Exceptions;
using ForkliftControlSystem.Domain.Utilities;

namespace ForkliftControlSystem.Infrastructure.Services;

internal class CommandService : ICommandService
{
    private List<ParsedCommand> ParseCommandInternal(string command)
    {
        var parsed = new List<ParsedCommand>();

        for (int i = 0; i < command.Length;)
        {
            char cmd = command[i];
            i++;

            string num = "";
            while (i < command.Length && char.IsDigit(command[i]))
            {
                num += command[i];
                i++;
            }

            if (!int.TryParse(num, out int value))
                throw new BadRequestException($"Invalid number after '{cmd}'");

            string desc = cmd switch
            {
                'F' => $"Move Forward by {value} metres",
                'B' => $"Move Backward by {value} metres",
                'L' => ValidateAndDescribeTurn(value, 'L'),
                'R' => ValidateAndDescribeTurn(value, 'R'),
                _ => throw new BadRequestException($"Unknown command: '{cmd}'")
            };

            parsed.Add(new ParsedCommand
            {
                Command = cmd,
                Value = value,
                ActionDescription = desc
            });
        }

        return parsed;
    }

    private string ValidateAndDescribeTurn(int value, char dir)
    {
        ValidationHelper.ValidateDegrees(value, dir);
        return dir == 'L' ? $"Turn Left by {value} degrees" : $"Turn Right by {value} degrees";
    }

    public List<string> ParseCommand(string command)
    {
        var steps = ParseCommandInternal(command);
        return steps.Select(s => s.ActionDescription).ToList();
    }

    public CommandResult ExecuteCommand(string command)
    {
        var steps = ParseCommandInternal(command);

        var result = new CommandResult();
        int x = 0, y = 0;
        Direction dir = Direction.North;

        foreach (var step in steps)
        {
            result.Actions.Add(step.ActionDescription);

            switch (step.Command)
            {
                case 'F': Move(ref x, ref y, dir, step.Value); break;
                case 'B': Move(ref x, ref y, dir, -step.Value); break;
                case 'L': dir = Turn(dir, -step.Value); break;
                case 'R': dir = Turn(dir, step.Value); break;
            }
        }

        result.FinalX = x;
        result.FinalY = y;
        result.FinalOrientation = dir.ToString();
        return result;
    }

    public CommandResult ExecuteCommandWithObstacles(string command, List<ObstacleDto> obstacles)
    {
        var steps = ParseCommandInternal(command);
        var result = new CommandResult();
        int x = 0, y = 0;
        Direction dir = Direction.North;

        var obstacleSet = new HashSet<string>(obstacles.Select(o => $"{o.X},{o.Y}"));

        foreach (var step in steps)
        {
            result.Actions.Add(step.ActionDescription);

            switch (step.Command)
            {
                case 'F':
                case 'B':
                    int moveAmount = (step.Command == 'F') ? step.Value : -step.Value;
                    for (int i = 0; i < Math.Abs(moveAmount); i++)
                    {
                        int nextX = x, nextY = y;
                        Move(ref nextX, ref nextY, dir, Math.Sign(moveAmount));
                        if (obstacleSet.Contains($"{nextX},{nextY}"))
                        {
                            result.Actions.Add($"Obstacle encountered at ({nextX}, {nextY}). Movement stopped.");
                            result.FinalX = x;
                            result.FinalY = y;
                            result.FinalOrientation = dir.ToString();
                            return result;
                        }
                        x = nextX;
                        y = nextY;
                    }
                    break;
                case 'L':
                    dir = Turn(dir, -step.Value);
                    break;
                case 'R':
                    dir = Turn(dir, step.Value);
                    break;
            }
        }

        result.FinalX = x;
        result.FinalY = y;
        result.FinalOrientation = dir.ToString();
        return result;
    }

    private static void Move(ref int x, ref int y, Direction dir, int distance)
    {
        switch (dir)
        {
            case Direction.North: y += distance; break;
            case Direction.East: x += distance; break;
            case Direction.South: y -= distance; break;
            case Direction.West: x -= distance; break;
        }
    }

    private static Direction Turn(Direction dir, int degrees)
    {
        int turns = (degrees / 90 + 4) % 4;
        return (Direction)(((int)dir + turns) % 4);
    }
}
