using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Commands.RefreshSession;

internal sealed class RefreshSessionCommandHandler(
    IUserService users,
    ISessionService sessions)
    : ICommandHandler<RefreshSessionCommand, AuthenticationResult>
{
    public async Task<Result<AuthenticationResult>> HandleAsync(
        RefreshSessionCommand command,
        CancellationToken ct = default)
    {
        var stored = await sessions.FindActiveAsync(command.RefreshToken, ct);

        if (stored is null)
        {
            return Result.Failure<AuthenticationResult>(ErrorCatalog.InvalidRefreshToken);
        }

        var user = await users.FindByIdAsync(stored.UserId, ct);

        if (user is null || !user.IsActive)
        {
            return Result.Failure<AuthenticationResult>(ErrorCatalog.UserInactive, $"userId={stored.UserId}");
        }

        sessions.Rotate(stored);

        var session = await sessions.IssueAsync(user, ct);

        return Result.Success(session);
    }
}
