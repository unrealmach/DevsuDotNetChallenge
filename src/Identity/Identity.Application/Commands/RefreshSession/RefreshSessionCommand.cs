using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Commands.RefreshSession;

public sealed record RefreshSessionCommand(string RefreshToken) : ICommand<AuthenticationResult>;
