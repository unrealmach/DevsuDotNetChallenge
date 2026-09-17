using Identity.Application.Dtos;

namespace Identity.Api.Contracts;

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
