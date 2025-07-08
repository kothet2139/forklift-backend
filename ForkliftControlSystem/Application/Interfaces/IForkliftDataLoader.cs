using ForkliftControlSystem.Domain.Entities;

namespace ForkliftControlSystem.Application.Interfaces
{
    public interface IForkliftDataLoader
    {
        Task<List<Forklift>> LoadForkliftDataAsync();
    }

}
