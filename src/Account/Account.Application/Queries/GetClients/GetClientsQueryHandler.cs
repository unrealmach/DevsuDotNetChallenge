using Account.Application.Abstractions.Messaging;
using Account.Application.Common;
using Account.Application.Dtos;
using Account.Application.Services;

namespace Account.Application.Queries.GetClients;

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
