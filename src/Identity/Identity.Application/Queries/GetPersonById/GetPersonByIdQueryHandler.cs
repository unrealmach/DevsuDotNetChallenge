using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Queries.GetPersonById;

internal sealed class GetPersonByIdQueryHandler(IPersonReadService persons)
    : IQueryHandler<GetPersonByIdQuery, PersonDto>
{
    public async Task<Result<PersonDto>> HandleAsync(GetPersonByIdQuery query, CancellationToken ct = default)
    {
        var person = await persons.GetByIdAsync(query.Id, ct);

        return person is null
            ? Result.Failure<PersonDto>(ErrorCatalog.PersonNotFound, $"personId={query.Id}")
            : Result.Success(person);
    }
}
