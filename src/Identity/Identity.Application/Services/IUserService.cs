using Identity.Domain.Entities;

namespace Identity.Application.Services;

internal interface IUserService
{
    Task<bool> EmailIsTakenAsync(string email, CancellationToken ct = default);

    Task<User> CreateAsync(string email, string password, string fullName, CancellationToken ct = default);

    Task<User?> FindByEmailAsync(string email, CancellationToken ct = default);

    Task<User?> FindByIdAsync(Guid id, CancellationToken ct = default);

    bool PasswordMatches(User user, string password);
}
