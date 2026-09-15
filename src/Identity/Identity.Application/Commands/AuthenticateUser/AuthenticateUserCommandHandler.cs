using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Commands.AuthenticateUser;

internal sealed class AuthenticateUserCommandHandler(
    IUserService users,
    ISessionService sessions)
    : ICommandHandler<AuthenticateUserCommand, AuthenticationResult>
{
    public async Task<Result<AuthenticationResult>> HandleAsync(
        AuthenticateUserCommand command,
        CancellationToken ct = default)
    {
        var user = await users.FindByEmailAsync(command.Email, ct);

        if (user is null || !users.PasswordMatches(user, command.Password))
        {
            return Result.Failure<AuthenticationResult>(
                ErrorCatalog.InvalidCredentials,
                $"email='{command.Email}'");
        }

        if (!user.IsActive)
        {
            return Result.Failure<AuthenticationResult>(ErrorCatalog.UserInactive, $"userId={user.Id}");
        }

        var session = await sessions.IssueAsync(user, ct);

        return Result.Success(session);
    }
}
