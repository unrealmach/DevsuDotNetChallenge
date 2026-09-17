using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;

namespace Identity.Application.Queries.GetClients;

internal sealed class GetClientsQueryHandler(IClientReadService clients)
    : IQueryHandler<GetClientsQuery, IReadOnlyList<ClientDto>>
{
    public async Task<Result<IReadOnlyList<ClientDto>>> HandleAsync(
        GetClientsQuery query,
        CancellationToken ct = default)
    {
        var all = await clients.GetAllAsync(ct);

        return Result.Success(all);
    }
}
