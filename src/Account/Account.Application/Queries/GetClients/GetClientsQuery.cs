using Account.Application.Abstractions.Messaging;
using Account.Application.Dtos;

namespace Account.Application.Queries.GetClients;

public sealed record GetClientsQuery : IQuery<IReadOnlyList<ClientDto>>;
