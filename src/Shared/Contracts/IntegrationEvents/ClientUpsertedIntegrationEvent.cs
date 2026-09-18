namespace Shared.Contracts.IntegrationEvents;

public sealed record ClientUpsertedIntegrationEvent(
    Guid Id,
    string Name,
    string Gender,
    string Age,
    string Identification,
    string Address,
    string Phone,
    string ClientId,
    string Status,
    DateTime OccurredAtUtc);
