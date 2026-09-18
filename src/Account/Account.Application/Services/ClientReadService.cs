using Account.Application.Dtos;
using Account.Application.Ports.Output.Read;

namespace Account.Application.Services;

internal sealed class ClientReadService(IClientReadRepository clients) : IClientReadService
{
    public Task<IReadOnlyList<ClientDto>> GetAllAsync(CancellationToken ct = default) =>
        clients.GetAllAsync(ct);

    public Task<ClientDto?> FindByIdAsync(Guid id, CancellationToken ct = default) =>
        clients.GetByIdAsync(id, ct);
}
