using Identity.Application.Abstractions.Messaging;

namespace Identity.Application.Commands.RevokeSessions;

public sealed record RevokeSessionsCommand(Guid UserId) : ICommand;
