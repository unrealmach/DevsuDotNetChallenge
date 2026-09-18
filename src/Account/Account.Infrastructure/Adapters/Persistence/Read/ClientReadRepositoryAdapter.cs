using System.Linq.Expressions;
using Account.Application.Dtos;
using Account.Application.Ports.Output.Read;
using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Adapters.Persistence.Read;

internal sealed class ClientReadRepositoryAdapter(AccountDbContext context) : IClientReadRepository
{
    public async Task<IReadOnlyList<ClientDto>> GetAllAsync(CancellationToken ct = default) =>
        await context.Clients
            .OrderBy(client => client.Name)
            .Select(ProjectToDto)
            .ToListAsync(ct);

    public async Task<ClientDto?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await context.Clients
            .Where(client => client.Id == id)
            .Select(ProjectToDto)
            .FirstOrDefaultAsync(ct);

    private static readonly Expression<Func<Client, ClientDto>> ProjectToDto =
        client => new ClientDto(
            client.Id,
            client.Name,
            client.Gender,
            client.Age,
            client.Identification,
            client.Address,
            client.Phone,
            client.ClientId,
            client.Status,
            client.CreatedAtUtc,
            client.UpdatedAtUtc);
}
