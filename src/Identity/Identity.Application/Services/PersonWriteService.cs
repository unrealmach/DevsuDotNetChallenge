using Identity.Application.Ports.Output.Write;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

internal sealed class PersonWriteService(IPersonWriteRepository persons) : IPersonWriteService
{
    public Task<Person?> FindByIdAsync(Guid id, CancellationToken ct = default) =>
        persons.GetByIdAsync(id, ct);

    public async Task<Person> CreateAsync(
        string name,
        string gender,
        string age,
        string identification,
        string address,
        string phone,
        CancellationToken ct = default)
    {
        var person = Person.Create()
            .WithName(name)
            .WithGender(gender)
            .WithAge(age)
            .WithIdentification(identification)
            .WithAddress(address)
            .WithPhone(phone);

        await persons.AddAsync(person, ct);

        return person;
    }

    public void Remove(Person person) => persons.Remove(person);
}
