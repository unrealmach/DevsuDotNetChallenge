namespace Identity.Application.Dtos;

public sealed record UserProfile(Guid Id, string Email, string FullName, string Role, DateTime CreatedAtUtc);

public sealed record AuthenticationResult(
    string AccessToken,
    DateTime ExpiresAtUtc,
    string RefreshToken,
    UserProfile User);
