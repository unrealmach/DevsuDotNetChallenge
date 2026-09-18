using Account.Application.Abstractions.Messaging;
using Account.Application.Common;
using Account.Application.Dtos;
using Account.Application.Services;

namespace Account.Application.Queries.GetReport;

internal sealed class GetReportQueryHandler(IMovementReadService movements)
    : IQueryHandler<GetReportQuery, IReadOnlyList<MovementDto>>
{
    public async Task<Result<IReadOnlyList<MovementDto>>> HandleAsync(
        GetReportQuery query,
        CancellationToken ct = default)
    {
        var rows = await movements.GetByRangeAsync(query.From, query.To, query.ClientId, ct);

        return Result.Success(rows);
    }
}
