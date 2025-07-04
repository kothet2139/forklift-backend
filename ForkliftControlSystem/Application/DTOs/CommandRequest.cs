namespace ForkliftControlSystem.Application.DTOs;

public class CommandRequest
{
    public string Command { get; set; } = string.Empty;
    public int ForkliftId { get; set; }
}
