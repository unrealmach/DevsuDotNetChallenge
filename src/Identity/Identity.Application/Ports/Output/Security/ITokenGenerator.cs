using Identity.Domain.Entities;

namespace Identity.Application.Ports.Output.Security;

internal sealed record AccessToken(string Value, DateTime ExpiresAtUtc);

internal interface ITokenGenerator
{
    AccessToken CreateAccessToken(User user);

    string CreateRefreshToken();

    DateTime GetRefreshTokenExpiration();
}
