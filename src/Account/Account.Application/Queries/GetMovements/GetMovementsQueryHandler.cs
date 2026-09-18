using Account.Application.Abstractions.Messaging;
using Account.Application.Common;
using Account.Application.Dtos;
using Account.Application.Services;

namespace Account.Application.Queries.GetMovements;

internal sealed class GetMovementsQueryHandler(IMovementReadService movements)
    : IQueryHandler<GetMovementsQuery, IReadOnlyList<MovementDto>>
{
    public async Task<Result<IReadOnlyList<MovementDto>>> HandleAsync(
        GetMovementsQuery query,
        CancellationToken ct = default)
    {
        var all = await movements.GetAllAsync(query.AccountId, ct);

        return Result.Success(all);
    }
}
