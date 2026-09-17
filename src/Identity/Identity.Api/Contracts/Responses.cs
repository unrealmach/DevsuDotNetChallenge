using Identity.Application.Dtos;

namespace Identity.Api.Contracts;

public sealed record PersonResponse(
    Guid Id,
    string Name,
    string Gender,
    string Age,
    string Identification,
    string Address,
    string Phone,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc)
{
    public static PersonResponse From(PersonDto person) => new(
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
