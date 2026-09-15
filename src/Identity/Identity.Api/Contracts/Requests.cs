using System.ComponentModel.DataAnnotations;
using Identity.Application.Commands.AuthenticateUser;
using Identity.Application.Commands.RefreshSession;
using Identity.Application.Commands.RegisterUser;

namespace Identity.Api.Contracts;

public sealed record RegisterRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(160)]
    public string Email { get; init; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string Password { get; init; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string FullName { get; init; } = string.Empty;

    public RegisterUserCommand ToCommand() => new(Email, Password, FullName);
}

public sealed record LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;

    public AuthenticateUserCommand ToCommand() => new(Email, Password);
}

public sealed record RefreshRequest
{
    [Required]
    public string RefreshToken { get; init; } = string.Empty;

    public RefreshSessionCommand ToCommand() => new(RefreshToken);
}
