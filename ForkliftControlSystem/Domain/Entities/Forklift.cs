using System.ComponentModel.DataAnnotations;

namespace ForkliftControlSystem.Domain.Entities;

public class Forklift
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }
    [MaxLength(50)]
    public string ModelNumber { get; set; }
    public DateTime ManufacturingDate { get; set; }

    public int Age => (int)((DateTime.Now - ManufacturingDate).TotalDays / 365.25);
}
