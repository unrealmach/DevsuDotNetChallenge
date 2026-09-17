using Identity.Application.Ports.Output.Write;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Adapters.Persistence.Write;

internal sealed class PersonWriteRepositoryAdapter(IdentityDbContext context) : IPersonWriteRepository
{
    public Task<Person?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Persons.FirstOrDefaultAsync(person => person.Id == id, ct);

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default) =>
        context.Persons.AnyAsync(person => person.Id == id, ct);

    public async Task AddAsync(Person person, CancellationToken ct = default) =>
        await context.Persons.AddAsync(person, ct);

    public void Remove(Person person) => context.Persons.Remove(person);
}
