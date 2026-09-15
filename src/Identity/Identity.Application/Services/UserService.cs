using Identity.Application.Ports.Output.Security;
using Identity.Application.Ports.Output.Write;
using Identity.Domain;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

internal sealed class UserService(IUserRepository users, IPasswordHasher passwordHasher) : IUserService
{
    public Task<bool> EmailIsTakenAsync(string email, CancellationToken ct = default) =>
        users.EmailExistsAsync(Normalize(email), ct);

    public async Task<User> CreateAsync(string email, string password, string fullName, CancellationToken ct = default)
    {
        var user = new User(Normalize(email), passwordHasher.Hash(password), fullName, Roles.User);
        await users.AddAsync(user, ct);

        return user;
    }

    public Task<User?> FindByEmailAsync(string email, CancellationToken ct = default) =>
        users.GetByEmailAsync(Normalize(email), ct);

    public Task<User?> FindByIdAsync(Guid id, CancellationToken ct = default) =>
        users.GetByIdAsync(id, ct);

    public bool PasswordMatches(User user, string password) =>
        passwordHasher.Verify(password, user.PasswordHash);

    private static string Normalize(string email) => email.Trim().ToLowerInvariant();
}
