using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Account.Infrastructure.Adapters.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AccountDbContext>
{
    public AccountDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Account")
                               ?? "Host=localhost;Port=5433;Database=devsu;Username=devsu;Password=devsu";

        var options = new DbContextOptionsBuilder<AccountDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable("__ef_migrations", AccountDbContext.Schema))
            .Options;

        return new AccountDbContext(options);
    }
}
