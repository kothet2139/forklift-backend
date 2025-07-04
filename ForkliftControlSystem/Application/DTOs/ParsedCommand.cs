namespace ForkliftControlSystem.Application.DTOs;

public class ParsedCommand
{
    public char Command { get; set; }
    public int Value { get; set; }
    public string ActionDescription { get; set; } = string.Empty;
}
