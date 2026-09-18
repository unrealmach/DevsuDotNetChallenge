using Account.Application.Dtos;

namespace Account.Application.Services;

internal interface IClientReadService
{
    Task<IReadOnlyList<ClientDto>> GetAllAsync(CancellationToken ct = default);

    Task<ClientDto?> FindByIdAsync(Guid id, CancellationToken ct = default);
}
