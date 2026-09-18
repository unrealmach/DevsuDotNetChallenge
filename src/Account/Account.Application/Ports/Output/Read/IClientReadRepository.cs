using Account.Application.Dtos;

namespace Account.Application.Ports.Output.Read;

internal interface IClientReadRepository
{
    Task<IReadOnlyList<ClientDto>> GetAllAsync(CancellationToken ct = default);

    Task<ClientDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
