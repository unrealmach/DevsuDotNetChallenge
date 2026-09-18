namespace Account.Domain.Entities;

public class Client : Person
{
    protected Client() { }

    protected Client(Guid id) : base(id) { }

    public string ClientId { get; private set; }
    public string Status { get; private set; }

    public static new Client Create() => new();

    public static new Client CreateWithId(Guid id) => new(id);

    public Client WithClientId(string clientId) { ClientId = clientId; return this; }
    public Client WithStatus(string status) { Status = status; return this; }
}