using ForkliftControlSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForkliftControlSystem.Infrastructure.Persistence;

public class ForkliftDbContext : DbContext
{
    public ForkliftDbContext(DbContextOptions<ForkliftDbContext> options)
        : base(options) { }

    public DbSet<Forklift> Forklifts => Set<Forklift>();
}
