using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Queries.GetPersons;

public sealed record GetPersonsQuery : IQuery<IReadOnlyList<PersonDto>>;
