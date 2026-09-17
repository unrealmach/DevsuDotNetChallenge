using Identity.Application.Dtos;
using Identity.Application.Ports.Output.Read;

namespace Identity.Application.Services;

internal sealed class PersonReadService(IPersonReadRepository persons) : IPersonReadService
{
    public Task<PersonDto?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        persons.GetByIdAsync(id, ct);

    public Task<IReadOnlyList<PersonDto>> GetAllAsync(CancellationToken ct = default) =>
        persons.GetAllAsync(ct);
}
