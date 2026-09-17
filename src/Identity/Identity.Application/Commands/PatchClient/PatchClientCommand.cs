using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Commands.PatchClient;

public sealed record PatchClientCommand(
    Guid Id,
    string? Name,
    string? Gender,
    string? Age,
    string? Identification,
    string? Address,
    string? Phone,
    string? Password,
    string? Status) : ICommand<ClientDto>;
