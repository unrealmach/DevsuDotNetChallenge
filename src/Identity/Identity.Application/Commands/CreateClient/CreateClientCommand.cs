using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Commands.CreateClient;

public sealed record CreateClientCommand(
    string Name,
    string Gender,
    string Age,
    string Identification,
    string Address,
    string Phone,
    string ClientId,
    string Password,
    string Status) : ICommand<ClientDto>;
