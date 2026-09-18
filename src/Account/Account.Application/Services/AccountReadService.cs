using Account.Application.Dtos;
using Account.Application.Ports.Output.Read;

namespace Account.Application.Services;

internal sealed class AccountReadService(IAccountReadRepository accounts) : IAccountReadService
{
    public Task<IReadOnlyList<AccountDto>> GetAllAsync(Guid? clientId, CancellationToken ct = default) =>
        accounts.GetAllAsync(clientId, ct);

    public Task<AccountDto?> FindByIdAsync(Guid id, CancellationToken ct = default) =>
        accounts.GetByIdAsync(id, ct);
}
