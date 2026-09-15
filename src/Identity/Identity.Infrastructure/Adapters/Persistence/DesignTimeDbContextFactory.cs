using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Infrastructure.Adapters.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Identity")
                               ?? "Host=localhost;Port=5433;Database=identity;Username=devsu;Password=devsu";

        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable("__ef_migrations", IdentityDbContext.Schema))
            .Options;

        return new IdentityDbContext(options);
    }
}
