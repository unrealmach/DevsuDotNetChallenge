using Identity.Application.Abstractions.Messaging;

namespace Identity.Application.Commands.DeleteClient;

public sealed record DeleteClientCommand(Guid Id) : ICommand;
