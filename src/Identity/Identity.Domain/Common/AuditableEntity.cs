namespace Identity.Domain.Common;

public abstract class AuditableEntity
{
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; private set; }

    protected void MarkAsUpdated() => UpdatedAtUtc = DateTime.UtcNow;
}
