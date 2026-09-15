using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IUserService users,
    ISessionService sessions)
    : ICommandHandler<RegisterUserCommand, AuthenticationResult>
{
    public async Task<Result<AuthenticationResult>> HandleAsync(
        RegisterUserCommand command,
        CancellationToken ct = default)
    {
        if (await users.EmailIsTakenAsync(command.Email, ct))
        {
            return Result.Failure<AuthenticationResult>(
                ErrorCatalog.UserEmailAlreadyRegistered,
                $"email='{command.Email}'");
        }

        var user = await users.CreateAsync(command.Email, command.Password, command.FullName, ct);
        var session = await sessions.IssueAsync(user, ct);

        return Result.Success(session);
    }
}
