using Identity.Application.Dtos;
using Identity.Application.Ports.Output.Read;

namespace Identity.Application.Services;

internal sealed class UserProfileService(IUserReadRepository users) : IUserProfileService
{
    public Task<UserProfile?> GetAsync(Guid userId, CancellationToken ct = default) =>
        users.GetProfileAsync(userId, ct);

    public Task<UserProfile?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        users.GetProfileByEmailAsync(email.Trim().ToLowerInvariant(), ct);
}
