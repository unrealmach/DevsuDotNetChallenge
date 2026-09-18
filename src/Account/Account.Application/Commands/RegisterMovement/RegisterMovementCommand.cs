using Account.Application.Abstractions.Messaging;
using Account.Application.Dtos;

namespace Account.Application.Commands.RegisterMovement;

// Value puede ser positivo (credito) o negativo (debito).
public sealed record RegisterMovementCommand(Guid AccountId, int Value) : ICommand<MovementDto>;
