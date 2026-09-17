using Identity.Application.Ports.Output.Write;

namespace Identity.Application.Services;

internal sealed class ClientValidationService(IClientWriteRepository clients) : IClientValidationService
{
    public Task<bool> ClientIdExistsAsync(string clientId, CancellationToken ct = default) =>
        clients.ClientIdExistsAsync(clientId, ct);

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default) =>
        clients.ExistsAsync(id, ct);
}
