using Identity.Application.Dtos;
using Identity.Application.Ports.Output.Security;
using Identity.Application.Ports.Output.Write;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

internal sealed class SessionService(IRefreshTokenRepository refreshTokens, ITokenGenerator tokenGenerator)
    : ISessionService
{
    public async Task<AuthenticationResult> IssueAsync(User user, CancellationToken ct = default)
    {
        var accessToken = tokenGenerator.CreateAccessToken(user);
        var refreshToken = new RefreshToken(
            user.Id,
            tokenGenerator.CreateRefreshToken(),
            tokenGenerator.GetRefreshTokenExpiration());

        await refreshTokens.AddAsync(refreshToken, ct);

        return new AuthenticationResult(
            accessToken.Value,
            accessToken.ExpiresAtUtc,
            refreshToken.Token,
            user.ToProfile());
    }

    public async Task<RefreshToken?> FindActiveAsync(string token, CancellationToken ct = default)
    {
        var stored = await refreshTokens.GetByTokenAsync(token, ct);

        return stored is not null && stored.IsActive(DateTime.UtcNow) ? stored : null;
    }

    public Task RevokeAllAsync(Guid userId, CancellationToken ct = default) =>
        refreshTokens.RevokeAllForUserAsync(userId, ct);

    public void Rotate(RefreshToken refreshToken) => refreshToken.Revoke();
}
