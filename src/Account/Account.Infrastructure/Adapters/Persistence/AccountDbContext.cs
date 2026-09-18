using System.Data.Common;
using Account.Application.Ports.Output.Write;
using Account.Domain.Entities;
using Account.Domain.Errors;
using Account.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using AccountEntity = Account.Domain.Entities.Account;

namespace Account.Infrastructure.Adapters.Persistence;

public sealed class AccountDbContext(DbContextOptions<AccountDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public const string Schema = "account";

    public DbSet<Person> Persons => Set<Person>();

    public DbSet<Client> Clients => Set<Client>();

    public DbSet<AccountEntity> Accounts => Set<AccountEntity>();

    public DbSet<History> Histories => Set<History>();

    public DbSet<Balance> Balances => Set<Balance>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new InfrastructureException(
                ErrorCatalog.DatabaseConcurrencyConflict,
                DescribeEntries(exception),
                exception);
        }
        catch (DbUpdateException exception)
        {
            throw new InfrastructureException(
                ErrorCatalog.DatabaseConstraintViolation,
                exception.InnerException?.Message ?? DescribeEntries(exception),
                exception);
        }
        catch (DbException exception)
        {
            throw new InfrastructureException(ErrorCatalog.DatabaseUnavailable, exception.Message, exception);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    private static string DescribeEntries(DbUpdateException exception) =>
        string.Join(", ", exception.Entries.Select(entry => entry.Metadata.DisplayName()));
}
