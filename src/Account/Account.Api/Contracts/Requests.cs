using System.ComponentModel.DataAnnotations;
using Account.Application.Commands.CreateAccount;
using Account.Application.Commands.RegisterMovement;

namespace Account.Api.Contracts;

public sealed record CreateAccountRequest
{
    [Required]
    public Guid ClientId { get; init; }

    [Required]
    [MaxLength(20)]
    public string Type { get; init; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Status { get; init; } = string.Empty;

    public CreateAccountCommand ToCommand() => new(ClientId, Type, Status);
}

public sealed record RegisterMovementRequest
{
    // Positivo = credito, negativo = debito.
    [Required]
    public int Value { get; init; }

    public RegisterMovementCommand ToCommand(Guid accountId) => new(accountId, Value);
}
