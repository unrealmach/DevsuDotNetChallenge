using Account.Domain.Common;
using Account.Domain.Errors;
using Account.Domain.Exceptions;

namespace Account.Domain.Entities;

public class Balance : AuditableEntity
{
    protected Balance() => Id = Guid.NewGuid();

    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Account Account { get; private set; } = null!;
    public int Value { get; private set; }

    public static Balance Create() => new();

    public Balance ForAccount(Guid accountId) { AccountId = accountId; return this; }

    // El saldo nunca puede quedar en negativo: es un invariante del dominio,
    // por eso se valida aca y no en la capa de aplicacion.
    public Balance Apply(int amount)
    {
        var next = Value + amount;

        if (next < 0)
        {
            throw new DomainException(ErrorCatalog.InsufficientBalance, $"saldoActual={Value}, movimiento={amount}");
        }

        Value = next;
        MarkAsUpdated();
        return this;
    }
}