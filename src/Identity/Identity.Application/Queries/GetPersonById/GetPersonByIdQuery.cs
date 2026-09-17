using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Queries.GetPersonById;

public sealed record GetPersonByIdQuery(Guid Id) : IQuery<PersonDto>;
