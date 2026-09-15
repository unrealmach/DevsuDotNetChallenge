using Identity.Application.Dtos;

namespace Identity.Application.Services;

internal interface IUserProfileService
{
    Task<UserProfile?> GetAsync(Guid userId, CancellationToken ct = default);

    Task<UserProfile?> GetByEmailAsync(string email, CancellationToken ct = default);
}
