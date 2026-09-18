namespace Account.Application.Dtos;

public sealed record MovementDto(
    Guid Id,
    Guid AccountId,
    string AccountType,
    Guid ClientId,
    string ClientName,
    DateTime Date,
    string Type,
    int Value,
    int AvailableBalance);
