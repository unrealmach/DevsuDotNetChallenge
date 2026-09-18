using Account.Application.Abstractions.Messaging;
using Account.Application.Common;
using Account.Application.Dtos;
using Account.Application.Services;
using Account.Domain.Errors;

namespace Account.Application.Commands.CreateAccount;

internal sealed class CreateAccountCommandHandler(IClientReadService clients, IAccountWriteService accounts)
    : ICommandHandler<CreateAccountCommand, AccountDto>
{
    public async Task<Result<AccountDto>> HandleAsync(CreateAccountCommand command, CancellationToken ct = default)
    {
        var client = await clients.FindByIdAsync(command.ClientId, ct);

        if (client is null)
        {
            return Result.Failure<AccountDto>(ErrorCatalog.ClientNotFound, $"clientId={command.ClientId}");
        }

        var account = await accounts.CreateAsync(command.ClientId, command.Type, command.Status, ct);

        var dto = new AccountDto(
            account.Id,
            client.Id,
            client.Name,
            account.Type,
            account.Status,
            0,
            account.CreatedAtUtc,
            account.UpdatedAtUtc);

        return Result.Success(dto);
    }
}
