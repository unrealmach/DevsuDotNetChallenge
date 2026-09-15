using Identity.Application.Dtos;

namespace Identity.Application.Ports.Output.Read;

internal interface IUserReadRepository
{
    Task<UserProfile?> GetProfileAsync(Guid userId, CancellationToken ct = default);

    Task<UserProfile?> GetProfileByEmailAsync(string email, CancellationToken ct = default);
}
