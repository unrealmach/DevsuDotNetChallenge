using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Application.Ports.Output.Security;
using Identity.Domain.Entities;
using Identity.Domain.Errors;
using Identity.Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Adapters.Security;

internal sealed class JwtTokenGeneratorAdapter(IOptions<JwtOptions> options) : ITokenGenerator
{
    private readonly JwtOptions _options = options.Value;

    public AccessToken CreateAccessToken(User user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.Role)
        };

        try
        {
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: credentials);

            return new AccessToken(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
        {
            throw new InfrastructureException(ErrorCatalog.TokenSigningFailed, $"userId={user.Id}", exception);
        }
    }

    public string CreateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    public DateTime GetRefreshTokenExpiration() => DateTime.UtcNow.AddDays(_options.RefreshTokenDays);
}
