using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Queries.GetClients;

public sealed record GetClientsQuery : IQuery<IReadOnlyList<ClientDto>>;
