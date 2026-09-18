using Account.Application.Dtos;

namespace Account.Application.Ports.Output.Read;

internal interface IAccountReadRepository
{
    Task<IReadOnlyList<AccountDto>> GetAllAsync(Guid? clientId, CancellationToken ct = default);

    Task<AccountDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
