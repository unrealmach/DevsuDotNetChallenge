using Identity.Application.Dtos;

namespace Identity.Application.Ports.Output.Read;

internal interface IPersonReadRepository
{
    Task<PersonDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<PersonDto>> GetAllAsync(CancellationToken ct = default);
}
