using Identity.Application.Dtos;
using Identity.Application.Ports.Output.Read;

namespace Identity.Application.Services;

internal sealed class ClientReadService(IClientReadRepository clients) : IClientReadService
{
    public Task<IReadOnlyList<ClientDto>> GetAllAsync(CancellationToken ct = default) =>
        clients.GetAllAsync(ct);
}
