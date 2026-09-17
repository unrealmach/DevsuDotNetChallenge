using Identity.Domain.Entities;

namespace Identity.Application.Services;

internal interface IPersonWriteService
{
    Task<Person?> FindByIdAsync(Guid id, CancellationToken ct = default);

    Task<Person> CreateAsync(
        string name,
        string gender,
        string age,
        string identification,
        string address,
        string phone,
        CancellationToken ct = default);

    void Remove(Person person);
}
