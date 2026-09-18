using Account.Application.Dtos;
using Account.Application.Ports.Output.Read;
using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AccountEntity = Account.Domain.Entities.Account;

namespace Account.Infrastructure.Adapters.Persistence.Read;

internal sealed class AccountReadRepositoryAdapter(AccountDbContext context) : IAccountReadRepository
{
    public async Task<IReadOnlyList<AccountDto>> GetAllAsync(Guid? clientId, CancellationToken ct = default) =>
        await Query()
            .Where(row => clientId == null || row.Account.ClientId == clientId)
            .OrderBy(row => row.Account.CreatedAtUtc)
            .Select(row => ToDto(row.Account, row.Balance))
            .ToListAsync(ct);

    public async Task<AccountDto?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await Query()
            .Where(row => row.Account.Id == id)
            .Select(row => ToDto(row.Account, row.Balance))
            .FirstOrDefaultAsync(ct);

    private IQueryable<AccountAndBalance> Query() =>
        context.Accounts
            .Include(account => account.Client)
            .Join(
                context.Balances,
                account => account.Id,
                balance => balance.AccountId,
                (account, balance) => new AccountAndBalance { Account = account, Balance = balance });

    private static AccountDto ToDto(AccountEntity account, Balance balance) => new(
        account.Id,
        account.ClientId,
        account.Client.Name,
        account.Type,
        account.Status,
        balance.Value,
        account.CreatedAtUtc,
        account.UpdatedAtUtc);

    private sealed class AccountAndBalance
    {
        public required AccountEntity Account { get; init; }
        public required Balance Balance { get; init; }
    }
}
