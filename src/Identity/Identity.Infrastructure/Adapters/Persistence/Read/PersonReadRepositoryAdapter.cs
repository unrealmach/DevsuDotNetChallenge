using System.Linq.Expressions;
using Identity.Application.Dtos;
using Identity.Application.Ports.Output.Read;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Adapters.Persistence.Read;

internal sealed class PersonReadRepositoryAdapter(IdentityReadOnlyDbContext context) : IPersonReadRepository
{
    public Task<PersonDto?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        context.Persons
            .Where(person => person.Id == id)
            .Select(ProjectToDto)
            .FirstOrDefaultAsync(ct);

    public async Task<IReadOnlyList<PersonDto>> GetAllAsync(CancellationToken ct = default) =>
        await context.Persons
            .OrderBy(person => person.Name)
            .Select(ProjectToDto)
            .ToListAsync(ct);

    private static readonly Expression<Func<Person, PersonDto>> ProjectToDto =
        person => new PersonDto(
            person.Id,
            person.Name,
            person.Gender,
            person.Age,
            person.Identification,
            person.Address,
            person.Phone,
            person.CreatedAtUtc,
            person.UpdatedAtUtc);
}
