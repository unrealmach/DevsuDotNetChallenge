using Account.Application.Abstractions.Messaging;
using Account.Application.Common;
using Account.Application.Dtos;
using Account.Application.Services;
using Account.Domain.Errors;

namespace Account.Application.Commands.RegisterMovement;

internal sealed class RegisterMovementCommandHandler(IAccountReadService accounts, IMovementService movements)
    : ICommandHandler<RegisterMovementCommand, MovementDto>
{
    public async Task<Result<MovementDto>> HandleAsync(RegisterMovementCommand command, CancellationToken ct = default)
    {
        var account = await accounts.FindByIdAsync(command.AccountId, ct);

        if (account is null)
        {
            return Result.Failure<MovementDto>(ErrorCatalog.AccountNotFound, $"accountId={command.AccountId}");
        }

        // RegisterAsync puede lanzar: UseCaseException (cuenta bloqueada) o
        // DomainException (saldo insuficiente) — las resuelve el GlobalExceptionHandler.
        var movement = await movements.RegisterAsync(account.Id, account.ClientId, command.Value, ct);

        var dto = new MovementDto(
            movement.Id,
            account.Id,
            account.Type,
            account.ClientId,
            account.ClientName,
            movement.Date,
            movement.Type,
            movement.Value,
            movement.AvailableBalance);

        return Result.Success(dto);
    }
}
