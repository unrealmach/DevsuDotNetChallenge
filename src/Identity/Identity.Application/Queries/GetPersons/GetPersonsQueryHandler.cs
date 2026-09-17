using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;

namespace Identity.Application.Queries.GetPersons;

internal sealed class GetPersonsQueryHandler(IPersonReadService persons)
    : IQueryHandler<GetPersonsQuery, IReadOnlyList<PersonDto>>
{
    public async Task<Result<IReadOnlyList<PersonDto>>> HandleAsync(
        GetPersonsQuery query,
        CancellationToken ct = default)
    {
        var all = await persons.GetAllAsync(ct);

        return Result.Success(all);
    }
}
