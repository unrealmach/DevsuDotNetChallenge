using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password, string FullName)
    : ICommand<AuthenticationResult>;
