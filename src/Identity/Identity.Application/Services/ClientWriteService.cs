using Identity.Application.Ports.Output.Security;
using Identity.Application.Ports.Output.Write;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

internal sealed class ClientWriteService(IClientWriteRepository clients, ICredentialEncryptor credentialEncryptor)
    : IClientWriteService
{
    public async Task<Client> CreateAsync(
        string name,
        string gender,
        string age,
        string identification,
        string address,
        string phone,
        string clientId,
        string password,
        string status,
        CancellationToken ct = default)
    {
        var client = Client.Create();
        client.WithName(name);
        client.WithGender(gender);
        client.WithAge(age);
        client.WithIdentification(identification);
        client.WithAddress(address);
        client.WithPhone(phone);
        client.WithClientId(clientId);
        client.WithPassword(credentialEncryptor.Encrypt(password));
        client.WithStatus(status);

        await clients.AddAsync(client, ct);

        return client;
    }

    public Task<Client?> FindByIdAsync(Guid id, CancellationToken ct = default) =>
        clients.GetByIdAsync(id, ct);

    public string EncryptPassword(string password) => credentialEncryptor.Encrypt(password);

    public void Remove(Client client) => clients.Remove(client);
}
