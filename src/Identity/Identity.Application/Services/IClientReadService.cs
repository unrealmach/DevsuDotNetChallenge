using Identity.Application.Dtos;

namespace Identity.Application.Services;

internal interface IClientReadService
{
    Task<IReadOnlyList<ClientDto>> GetAllAsync(CancellationToken ct = default);
}
