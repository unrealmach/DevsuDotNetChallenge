using Identity.Application.Ports.Output.Write;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Adapters.Persistence.Write;

internal sealed class RefreshTokenRepositoryAdapter(IdentityDbContext context) : IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default) =>
        context.RefreshTokens.FirstOrDefaultAsync(refreshToken => refreshToken.Token == token, ct);

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default) =>
        await context.RefreshTokens.AddAsync(refreshToken, ct);

    public async Task RevokeAllForUserAsync(Guid userId, CancellationToken ct = default)
    {
        var active = await context.RefreshTokens
            .Where(token => token.UserId == userId && token.RevokedAtUtc == null)
            .ToListAsync(ct);

        foreach (var token in active)
        {
            token.Revoke();
        }
    }
}
