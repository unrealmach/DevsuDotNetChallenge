using System.Data.Common;
using Identity.Application.Ports.Output.Write;
using Identity.Domain.Entities;
using Identity.Domain.Errors;
using Identity.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Adapters.Persistence;

public sealed class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public const string Schema = "identity";

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    private static string DescribeEntries(DbUpdateException exception) =>
        string.Join(", ", exception.Entries.Select(entry => entry.Metadata.DisplayName()));
}
