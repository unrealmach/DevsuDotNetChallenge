using Identity.Application.Dtos;

namespace Identity.Application.Ports.Output.Read;

internal interface IClientReadRepository
{
    Task<IReadOnlyList<ClientDto>> GetAllAsync(CancellationToken ct = default);
}
