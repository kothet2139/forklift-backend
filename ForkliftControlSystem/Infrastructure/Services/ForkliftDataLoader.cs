using ForkliftControlSystem.Application.Interfaces;
using ForkliftControlSystem.Domain.Entities;
using System.Text.Json;

namespace ForkliftControlSystem.Infrastructure.Services
{
    public class FileForkliftDataLoader : IForkliftDataLoader
    {
        private readonly IHostEnvironment _env;
        public FileForkliftDataLoader(IHostEnvironment env)
        {
            _env = env;
        }
        public async Task<List<Forklift>> LoadForkliftDataAsync()
        {
            var path = Path.Combine(_env.ContentRootPath, "Infrastructure\\Data", "fleet.json");
            var json = await File.ReadAllTextAsync(path);

            var forklifts = JsonSerializer.Deserialize<List<Forklift>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return forklifts!;
        }
    }

}
