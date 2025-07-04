namespace ForkliftControlSystem.Application.DTOs;

public class ForkliftDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ModelNumber { get; set; } = string.Empty;
    public DateTime ManufacturingDate { get; set; }
    public int Age { get; set; }
}
