namespace ForkliftControlSystem.Application.DTOs;

public class CommandResult
{
    public List<string> Actions { get; set; } = new();
    public int FinalX { get; set; }
    public int FinalY { get; set; }
    public string FinalOrientation { get; set; } = "North";
}
