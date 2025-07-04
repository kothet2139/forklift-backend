using ForkliftControlSystem.Domain.Entities;

namespace ForkliftControlSystem.Application.Interfaces.Repositories;

public interface IForkliftRepository
{
    IQueryable<Forklift> GetAllAsync();
    Task AddRangeAsync(IEnumerable<Forklift> forklifts);
    Task<bool> AnyAsync();
    Task SaveChangesAsync();
}
