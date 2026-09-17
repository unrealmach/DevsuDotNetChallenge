using Identity.Domain.Entities;

namespace Identity.Application.Ports.Output.Write;

internal interface IPersonWriteRepository
{
    Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);

    Task AddAsync(Person person, CancellationToken ct = default);

    void Remove(Person person);
}
