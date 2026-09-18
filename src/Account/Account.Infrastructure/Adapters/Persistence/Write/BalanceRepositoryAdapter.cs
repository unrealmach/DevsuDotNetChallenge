using Account.Application.Ports.Output.Write;
using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Adapters.Persistence.Write;

internal sealed class BalanceRepositoryAdapter(AccountDbContext context) : IBalanceRepository
{
    public Task<Balance?> GetByAccountIdAsync(Guid accountId, CancellationToken ct = default) =>
        context.Balances.FirstOrDefaultAsync(balance => balance.AccountId == accountId, ct);

    public async Task AddAsync(Balance balance, CancellationToken ct = default) =>
        await context.Balances.AddAsync(balance, ct);
}
