using Account.Domain.Common;

namespace Account.Domain.Entities;

public class Account : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public Client Client { get; private set; } = null!;
    public string Type { get; private set; }
    public string Status { get; private set; }
}