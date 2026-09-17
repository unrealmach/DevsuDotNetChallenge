using Identity.Application.Dtos;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

internal static class PersonMapper
{
    public static PersonDto ToDto(this Person person) => new(
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
