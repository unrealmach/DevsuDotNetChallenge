using Identity.Application.Dtos;

namespace Identity.Api.Contracts;

public sealed record UserResponse(Guid Id, string Email, string FullName, string Role, DateTime CreatedAtUtc)
{
    public static UserResponse From(UserProfile profile) =>
        new(profile.Id, profile.Email, profile.FullName, profile.Role, profile.CreatedAtUtc);
}

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    string RefreshToken,
    UserResponse User)
{
    public static AuthResponse From(AuthenticationResult result) =>
        new(result.AccessToken, result.ExpiresAtUtc, result.RefreshToken, UserResponse.From(result.User));
}
