namespace Account.Application.Dtos;

public sealed record AccountDto(
    Guid Id,
    Guid ClientId,
    string ClientName,
    string Type,
    string Status,
    int Balance,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);
