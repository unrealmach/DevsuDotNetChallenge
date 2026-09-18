using Identity.Application.Dtos;
using Identity.Domain.Entities;
using Shared.Contracts.IntegrationEvents;

namespace Identity.Application.Services;

internal static class ClientMapper
{
    public static ClientDto ToDto(this Client client) => new(
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

    public static ClientUpsertedIntegrationEvent ToUpsertedIntegrationEvent(this Client client) => new(
        client.Id,
        client.Name,
        client.Gender,
        client.Age,
        client.Identification,
        client.Address,
        client.Phone,
        client.ClientId,
        client.Status,
        DateTime.UtcNow);
}
