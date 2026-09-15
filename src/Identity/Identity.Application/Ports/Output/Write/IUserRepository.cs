using Identity.Domain.Entities;

namespace Identity.Application.Ports.Output.Write;

internal interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);

    Task AddAsync(User user, CancellationToken ct = default);
}
