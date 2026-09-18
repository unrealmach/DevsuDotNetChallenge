using AccountEntity = Account.Domain.Entities.Account;

namespace Account.Application.Services;

internal interface IAccountWriteService
{
    Task<AccountEntity> CreateAsync(Guid clientId, string type, string status, CancellationToken ct = default);
}
