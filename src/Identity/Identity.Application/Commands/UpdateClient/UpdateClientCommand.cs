using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Commands.UpdateClient;

public sealed record UpdateClientCommand(
    Guid Id,
    string Name,
    string Gender,
    string Age,
    string Identification,
    string Address,
    string Phone,
    string Status) : ICommand<ClientDto>;
