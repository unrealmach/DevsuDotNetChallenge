using Account.Application.Dtos;
using Account.Application.Ports.Output.Read;

namespace Account.Application.Services;

internal sealed class MovementReadService(IMovementReadRepository movements) : IMovementReadService
{
    public Task<IReadOnlyList<MovementDto>> GetAllAsync(Guid? accountId, CancellationToken ct = default) =>
        movements.GetAllAsync(accountId, ct);

    public Task<IReadOnlyList<MovementDto>> GetByRangeAsync(
        DateTime from,
        DateTime to,
        Guid? clientId,
        CancellationToken ct = default) =>
        movements.GetByRangeAsync(from, to, clientId, ct);
}
