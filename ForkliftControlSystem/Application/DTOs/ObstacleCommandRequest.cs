namespace ForkliftControlSystem.Application.DTOs
{
    public class ObstacleCommandRequest : CommandRequest
    {
        public List<ObstacleDto>? Obstacles { get; set; } = new();
    }
}
