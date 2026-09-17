using Identity.Domain.Entities;

namespace Identity.Application.Services;

internal interface IClientWriteService
{
    Task<Client> CreateAsync(
        string name,
        string gender,
        string age,
        string identification,
        string address,
        string phone,
        string clientId,
        string password,
        string status,
        CancellationToken ct = default);

    Task<Client?> FindByIdAsync(Guid id, CancellationToken ct = default);

    string EncryptPassword(string password);

    void Remove(Client client);
}
