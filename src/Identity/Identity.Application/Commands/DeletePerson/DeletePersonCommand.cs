using Identity.Application.Abstractions.Messaging;

namespace Identity.Application.Commands.DeletePerson;

public sealed record DeletePersonCommand(Guid Id) : ICommand;
