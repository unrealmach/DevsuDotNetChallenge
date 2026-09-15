using Identity.Application.Ports.Output.Write;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Adapters.Persistence.Write;

internal sealed class UserRepositoryAdapter(IdentityDbContext context) : IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Users.FirstOrDefaultAsync(user => user.Id == id, ct);

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        context.Users.FirstOrDefaultAsync(user => user.Email == email, ct);

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct = default) =>
        context.Users.AnyAsync(user => user.Email == email, ct);

    public async Task AddAsync(User user, CancellationToken ct = default) =>
        await context.Users.AddAsync(user, ct);
}
