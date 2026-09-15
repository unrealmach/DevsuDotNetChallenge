using Identity.Application.Dtos;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

internal interface ISessionService
{
    Task<AuthenticationResult> IssueAsync(User user, CancellationToken ct = default);

    Task<RefreshToken?> FindActiveAsync(string token, CancellationToken ct = default);

    Task RevokeAllAsync(Guid userId, CancellationToken ct = default);

    void Rotate(RefreshToken refreshToken);
}
