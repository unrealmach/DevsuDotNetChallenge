using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Commands.CreateClient;

internal sealed class CreateClientCommandHandler(IClientValidationService validation, IClientWriteService clients)
    : ICommandHandler<CreateClientCommand, ClientDto>
{
    public async Task<Result<ClientDto>> HandleAsync(CreateClientCommand command, CancellationToken ct = default)
    {
        if (await validation.ClientIdExistsAsync(command.ClientId, ct))
        {
            return Result.Failure<ClientDto>(ErrorCatalog.ClientIdAlreadyRegistered, $"clientId={command.ClientId}");
        }

        var client = await clients.CreateAsync(
            command.Name,
            command.Gender,
            command.Age,
            command.Identification,
            command.Address,
            command.Phone,
            command.ClientId,
            command.Password,
            command.Status,
            ct);

        await clients.PublishUpsertedAsync(client, ct);

        return Result.Success(client.ToDto());
    }
}
