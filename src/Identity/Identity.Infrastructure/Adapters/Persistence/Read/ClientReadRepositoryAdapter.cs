using System.Linq.Expressions;
using Identity.Application.Dtos;
using Identity.Application.Ports.Output.Read;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Adapters.Persistence.Read;

internal sealed class ClientReadRepositoryAdapter(IdentityDbContext context) : IClientReadRepository
{
    public async Task<IReadOnlyList<ClientDto>> GetAllAsync(CancellationToken ct = default) =>
        await context.Clients
            .OrderBy(client => client.Name)
            .Select(ProjectToDto)
            .ToListAsync(ct);

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
