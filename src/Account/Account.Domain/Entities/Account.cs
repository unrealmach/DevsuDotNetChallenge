using Account.Domain.Common;

namespace Account.Domain.Entities;

public class Account : AuditableEntity
{
    protected Account() => Id = Guid.NewGuid();

    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public Client Client { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public string Status { get; private set; } = null!;

    public static Account Create() => new();

    public Account WithClientId(Guid clientId) { ClientId = clientId; return this; }
    public Account WithType(string type) { Type = type; return this; }
    public Account WithStatus(string status) { Status = status; return this; }
}