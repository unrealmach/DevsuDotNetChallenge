using Account.Application.Dtos;

namespace Account.Api.Contracts;

public sealed record AccountResponse(
    Guid Id,
    Guid ClientId,
    string ClientName,
    string Type,
    string Status,
    int Balance,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc)
{
    public static AccountResponse From(AccountDto account) => new(
        account.Id,
        account.ClientId,
        account.ClientName,
        account.Type,
        account.Status,
        account.Balance,
        account.CreatedAtUtc,
        account.UpdatedAtUtc);
}

public sealed record ClientResponse(
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
    DateTime? UpdatedAtUtc)
{
    public static ClientResponse From(ClientDto client) => new(
        client.Id,
        client.Name,
        client.Gender,
        client.Age,
        client.Identification,
        client.Address,
        client.Phone,
        client.ClientId,
        client.Status,
        client.CreatedAtUtc,
        client.UpdatedAtUtc);
}

public sealed record MovementResponse(
    Guid Id,
    Guid AccountId,
    string AccountType,
    Guid ClientId,
    string ClientName,
    DateTime Date,
    string Type,
    int Value,
    int AvailableBalance)
{
    public static MovementResponse From(MovementDto movement) => new(
        movement.Id,
        movement.AccountId,
        movement.AccountType,
        movement.ClientId,
        movement.ClientName,
        movement.Date,
        movement.Type,
        movement.Value,
        movement.AvailableBalance);
}
