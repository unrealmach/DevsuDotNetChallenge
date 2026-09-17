using Identity.Application.Dtos;

namespace Identity.Application.Services;

internal interface IPersonReadService
{
    Task<PersonDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<PersonDto>> GetAllAsync(CancellationToken ct = default);
}
