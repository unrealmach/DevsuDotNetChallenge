using Account.Application.Dtos;

namespace Account.Application.Services;

internal interface IMovementReadService
{
    Task<IReadOnlyList<MovementDto>> GetAllAsync(Guid? accountId, CancellationToken ct = default);

    Task<IReadOnlyList<MovementDto>> GetByRangeAsync(
        DateTime from,
        DateTime to,
        Guid? clientId,
        CancellationToken ct = default);
}
