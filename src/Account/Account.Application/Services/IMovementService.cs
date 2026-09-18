using Account.Domain.Entities;

namespace Account.Application.Services;

internal interface IMovementService
{
    Task<History> RegisterAsync(Guid accountId, Guid clientId, int value, CancellationToken ct = default);
}
