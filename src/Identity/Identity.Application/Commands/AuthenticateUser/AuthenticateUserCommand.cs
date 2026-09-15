using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Commands.AuthenticateUser;

public sealed record AuthenticateUserCommand(string Email, string Password)
    : ICommand<AuthenticationResult>;
