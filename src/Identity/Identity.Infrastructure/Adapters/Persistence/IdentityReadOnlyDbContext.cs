using Identity.Domain.Entities;
using Identity.Infrastructure.Adapters.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Adapters.Persistence;

public sealed class IdentityReadOnlyDbContext(DbContextOptions<IdentityReadOnlyDbContext> options) : DbContext(options)
{
    public DbSet<Person> Persons => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(IdentityDbContext.Schema);
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
