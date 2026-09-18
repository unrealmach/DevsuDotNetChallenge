using Account.Domain.Common;

namespace Account.Domain.Entities;

public class Balance : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Account Account { get; private set; } = null!;
    public int Value { get; private set; }
}