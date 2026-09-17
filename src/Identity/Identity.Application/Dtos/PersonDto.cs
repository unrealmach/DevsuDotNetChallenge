namespace Identity.Application.Dtos;

public sealed record PersonDto(
    Guid Id,
    string Name,
    string Gender,
    string Age,
    string Identification,
    string Address,
    string Phone,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);
