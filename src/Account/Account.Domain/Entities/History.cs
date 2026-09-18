using Account.Domain.Common;

namespace Account.Domain.Entities;

public class History : AuditableEntity
{
    protected History() => Id = Guid.NewGuid();

    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public Client Client { get; private set; } = null!;
    public Guid AccountId { get; private set; }
    public Account Account { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public int Value { get; private set; }
    public string Type { get; private set; } = null!;

    // Saldo resultante despues de aplicar este movimiento. Se guarda "congelado"
    // en el momento del movimiento para que el reporte no tenga que recalcularlo.
    public int AvailableBalance { get; private set; }

    public static History Create() => new();

    public History ForClient(Guid clientId) { ClientId = clientId; return this; }
    public History ForAccount(Guid accountId) { AccountId = accountId; return this; }
    public History WithDate(DateTime date) { Date = date; return this; }
    public History WithValue(int value) { Value = value; return this; }
    public History WithType(string type) { Type = type; return this; }
    public History WithAvailableBalance(int balance) { AvailableBalance = balance; return this; }
}