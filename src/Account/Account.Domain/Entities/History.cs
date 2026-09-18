using Account.Domain.Common;

namespace Account.Domain.Entities;

public class History : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public Client Client { get; private set; } = null!;
    public Guid AccountId { get; private set; }
    public Account Account { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public int Value { get; private set; }
    public string Type { get; private set; }
}