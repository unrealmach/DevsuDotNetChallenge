using Account.Application.Dtos;

namespace Account.Application.Ports.Output.Read;

internal interface IMovementReadRepository
{
    Task<IReadOnlyList<MovementDto>> GetAllAsync(Guid? accountId, CancellationToken ct = default);

    Task<IReadOnlyList<MovementDto>> GetByRangeAsync(
        DateTime from,
        DateTime to,
        Guid? clientId,
        CancellationToken ct = default);
}
