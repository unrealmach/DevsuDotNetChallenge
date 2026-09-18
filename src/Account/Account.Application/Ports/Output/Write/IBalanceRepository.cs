using Account.Domain.Entities;

namespace Account.Application.Ports.Output.Write;

internal interface IBalanceRepository
{
    Task<Balance?> GetByAccountIdAsync(Guid accountId, CancellationToken ct = default);

    Task AddAsync(Balance balance, CancellationToken ct = default);
}
