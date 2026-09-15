using Identity.Domain.Errors;
using Identity.Domain.Exceptions;

namespace Identity.Domain.Entities;

public sealed class RefreshToken
{
    private RefreshToken()
    {
    }

    public RefreshToken(Guid userId, string token, DateTime expiresAtUtc)
    {
        if (expiresAtUtc <= DateTime.UtcNow)
        {
            throw new DomainException(ErrorCatalog.RefreshTokenInvalidExpiration, $"expiresAtUtc={expiresAtUtc:O}");
        }

        Id = Guid.NewGuid();
        UserId = userId;
        Token = token;
        ExpiresAtUtc = expiresAtUtc;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }

    public bool IsActive(DateTime utcNow) => RevokedAtUtc is null && utcNow < ExpiresAtUtc;

    public void Revoke()
    {
        if (RevokedAtUtc is not null)
        {
            throw new DomainException(ErrorCatalog.RefreshTokenAlreadyRevoked, $"refreshTokenId={Id}");
        }

        RevokedAtUtc = DateTime.UtcNow;
    }
}
