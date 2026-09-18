using AccountEntity = Account.Domain.Entities.Account;

namespace Account.Application.Ports.Output.Write;

internal interface IAccountWriteRepository
{
    Task AddAsync(AccountEntity account, CancellationToken ct = default);
}
