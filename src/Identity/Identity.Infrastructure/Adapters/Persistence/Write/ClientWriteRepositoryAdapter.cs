using Identity.Application.Ports.Output.Write;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Adapters.Persistence.Write;

internal sealed class ClientWriteRepositoryAdapter(IdentityDbContext context) : IClientWriteRepository
{
    public Task<bool> ClientIdExistsAsync(string clientId, CancellationToken ct = default) =>
        context.Clients.AnyAsync(client => client.ClientId == clientId, ct);

    public Task<Client?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Clients.FirstOrDefaultAsync(client => client.Id == id, ct);

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default) =>
        context.Clients.AnyAsync(client => client.Id == id, ct);

    public async Task AddAsync(Client client, CancellationToken ct = default) =>
        await context.Clients.AddAsync(client, ct);

    public void Remove(Client client) => context.Clients.Remove(client);
}
