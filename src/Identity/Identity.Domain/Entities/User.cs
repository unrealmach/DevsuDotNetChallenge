using Identity.Domain.Errors;
using Identity.Domain.Exceptions;

namespace Identity.Domain.Entities;

public sealed class User
{
    private User()
    {
    }

    public User(string email, string passwordHash, string fullName, string role)
    {
        var normalizedEmail = email?.Trim().ToLowerInvariant() ?? string.Empty;

        if (normalizedEmail.Length == 0 || !normalizedEmail.Contains('@'))
        {
            throw new DomainException(ErrorCatalog.UserInvalidEmail, $"email='{email}'");
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new DomainException(ErrorCatalog.UserInvalidName);
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new DomainException(ErrorCatalog.UserEmptyPasswordHash, $"email='{normalizedEmail}'");
        }

        if (!Roles.IsValid(role))
        {
            throw new DomainException(ErrorCatalog.UserInvalidRole, $"role='{role}'");
        }

        Id = Guid.NewGuid();
        Email = normalizedEmail;
        PasswordHash = passwordHash;
        FullName = fullName.Trim();
        Role = role;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public string Role { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
        {
            throw new DomainException(ErrorCatalog.UserEmptyPasswordHash, $"userId={Id}");
        }

        PasswordHash = newPasswordHash;
    }

    public void Deactivate() => IsActive = false;
}
