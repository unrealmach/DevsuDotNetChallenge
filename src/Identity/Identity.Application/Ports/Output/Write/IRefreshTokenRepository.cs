using Identity.Domain.Entities;

namespace Identity.Application.Ports.Output.Write;

internal interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);

    Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default);

    Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default);
}
