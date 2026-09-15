using Identity.Application.Dtos;
using Identity.Application.Ports.Output.Read;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Adapters.Persistence.Read;

internal sealed class UserReadRepositoryAdapter(IdentityDbContext context) : IUserReadRepository
{
    public Task<UserProfile?> GetProfileAsync(Guid userId, CancellationToken ct = default) =>
        Project(context.Users.Where(user => user.Id == userId)).FirstOrDefaultAsync(ct);

    public Task<UserProfile?> GetProfileByEmailAsync(string email, CancellationToken ct = default) =>
        Project(context.Users.Where(user => user.Email == email)).FirstOrDefaultAsync(ct);

    private static IQueryable<UserProfile> Project(IQueryable<User> users) =>
        users
            .AsNoTracking()
            .Select(user => new UserProfile(
                user.Id,
                user.Email,
                user.FullName,
                user.Role,
                user.CreatedAtUtc));
}
