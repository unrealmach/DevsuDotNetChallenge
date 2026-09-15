using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Commands.RevokeSessions;

internal sealed class RevokeSessionsCommandHandler(
    IUserService users,
    ISessionService sessions)
    : ICommandHandler<RevokeSessionsCommand>
{
    public async Task<Result> HandleAsync(RevokeSessionsCommand command, CancellationToken ct = default)
    {
        var user = await users.FindByIdAsync(command.UserId, ct);

        if (user is null)
        {
            return Result.Failure(ErrorCatalog.UserNotFound, $"userId={command.UserId}");
        }

        await sessions.RevokeAllAsync(command.UserId, ct);

        return Result.Success();
    }
}
