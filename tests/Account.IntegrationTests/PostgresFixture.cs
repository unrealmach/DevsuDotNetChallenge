using Account.Infrastructure.Adapters.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Account.IntegrationTests;

public sealed class PostgresFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine").Build();

    public AccountDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AccountDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        return new AccountDbContext(options);
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        await using var context = CreateContext();
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync() => await _postgres.DisposeAsync();
}
