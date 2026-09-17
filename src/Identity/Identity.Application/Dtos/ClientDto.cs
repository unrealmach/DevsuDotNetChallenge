namespace Identity.Application.Dtos;

public sealed record ClientDto(
    Guid Id,
    string Name,
    string Gender,
    string Age,
    string Identification,
    string Address,
    string Phone,
    string ClientId,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);
