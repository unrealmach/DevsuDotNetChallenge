using Identity.Application.Ports.Output.Write;

namespace Identity.Application.Services;

internal sealed class PersonValidationService(IPersonWriteRepository persons) : IPersonValidationService
{
    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default) =>
        persons.ExistsAsync(id, ct);
}
