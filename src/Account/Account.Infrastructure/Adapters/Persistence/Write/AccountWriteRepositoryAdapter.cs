using Account.Application.Ports.Output.Write;
using AccountEntity = Account.Domain.Entities.Account;

namespace Account.Infrastructure.Adapters.Persistence.Write;

internal sealed class AccountWriteRepositoryAdapter(AccountDbContext context) : IAccountWriteRepository
{
    public async Task AddAsync(AccountEntity account, CancellationToken ct = default) =>
        await context.Accounts.AddAsync(account, ct);
}
