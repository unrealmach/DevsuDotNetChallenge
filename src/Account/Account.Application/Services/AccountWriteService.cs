using Account.Application.Ports.Output.Write;
using Account.Domain.Entities;
using AccountEntity = Account.Domain.Entities.Account;

namespace Account.Application.Services;

internal sealed class AccountWriteService(IAccountWriteRepository accounts, IBalanceRepository balances)
    : IAccountWriteService
{
    public async Task<AccountEntity> CreateAsync(
        Guid clientId,
        string type,
        string status,
        CancellationToken ct = default)
    {
        var account = AccountEntity.Create().WithClientId(clientId).WithType(type).WithStatus(status);
        await accounts.AddAsync(account, ct);

        // Toda cuenta nace con un Balance en 0 — sin esto RegisterMovement no
        var balance = Balance.Create().ForAccount(account.Id);
        await balances.AddAsync(balance, ct);

        return account;
    }
}
