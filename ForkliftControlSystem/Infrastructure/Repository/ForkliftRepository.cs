using ForkliftControlSystem.Application.Interfaces.Repositories;
using ForkliftControlSystem.Domain.Entities;
using ForkliftControlSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ForkliftControlSystem.Infrastructure.Repositories;

public class ForkliftRepository : IForkliftRepository
{
    private readonly ForkliftDbContext _context;

    public ForkliftRepository(ForkliftDbContext context)
    {
        _context = context;
    }

    public IQueryable<Forklift> GetAllAsync()
    {
        return _context.Forklifts.AsNoTracking();
    }

    public async Task AddRangeAsync(IEnumerable<Forklift> forklifts)
    {
        await _context.Forklifts.AddRangeAsync(forklifts);
    }

    public async Task<bool> AnyAsync()
    {
        return await _context.Forklifts.AnyAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
