using Account.Application.Exceptions;
using Account.Application.Ports.Output.Locking;
using Account.Application.Ports.Output.Write;
using Account.Domain.Entities;
using Account.Domain.Errors;

namespace Account.Application.Services;

internal sealed class MovementService(
    IDistributedLock locks,
    IBalanceRepository balances,
    IHistoryWriteRepository histories,
    IUnitOfWork unitOfWork)
    : IMovementService
{
    private static readonly TimeSpan LockTtl = TimeSpan.FromSeconds(10);

    public async Task<History> RegisterAsync(
        Guid accountId,
        Guid clientId,
        int value,
        CancellationToken ct = default)
    {
        await using var handle = await locks.AcquireAsync($"account-lock:{accountId}", LockTtl, ct);

        if (handle is null)
        {
            throw new UseCaseException(ErrorCatalog.AccountLocked, $"accountId={accountId}");
        }

        var balance = await balances.GetByAccountIdAsync(accountId, ct)
            ?? throw new UseCaseException(ErrorCatalog.AccountWithoutBalance, $"accountId={accountId}");

        balance.Apply(value);

        var movement = History.Create()
            .ForClient(clientId)
            .ForAccount(accountId)
            .WithDate(DateTime.UtcNow)
            .WithValue(value)
            .WithType(value >= 0 ? "CREDITO" : "DEBITO")
            .WithAvailableBalance(balance.Value);

        await histories.AddAsync(movement, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return movement;
    }
}
