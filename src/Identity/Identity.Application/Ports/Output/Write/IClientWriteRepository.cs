using Identity.Domain.Entities;

namespace Identity.Application.Ports.Output.Write;

internal interface IClientWriteRepository
{
    Task<bool> ClientIdExistsAsync(string clientId, CancellationToken ct = default);

    Task<Client?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(Client client, CancellationToken ct = default);

    void Remove(Client client);
}
