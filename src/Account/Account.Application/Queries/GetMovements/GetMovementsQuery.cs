using Account.Application.Abstractions.Messaging;
using Account.Application.Dtos;

namespace Account.Application.Queries.GetMovements;

public sealed record GetMovementsQuery(Guid? AccountId) : IQuery<IReadOnlyList<MovementDto>>;
