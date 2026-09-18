using Account.Application.Dtos;

namespace Account.Application.Services;

internal interface IAccountReadService
{
    Task<IReadOnlyList<AccountDto>> GetAllAsync(Guid? clientId, CancellationToken ct = default);

    Task<AccountDto?> FindByIdAsync(Guid id, CancellationToken ct = default);
}
