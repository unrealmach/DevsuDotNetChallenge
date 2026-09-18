using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Commands.UpdateClient;

internal sealed class UpdateClientCommandHandler(IClientWriteService clients)
    : ICommandHandler<UpdateClientCommand, ClientDto>
{
    public async Task<Result<ClientDto>> HandleAsync(UpdateClientCommand command, CancellationToken ct = default)
    {
        var client = await clients.FindByIdAsync(command.Id, ct);

        if (client is null)
        {
            return Result.Failure<ClientDto>(ErrorCatalog.ClientNotFound, $"clientId={command.Id}");
        }

        client.WithName(command.Name);
        client.WithGender(command.Gender);
        client.WithAge(command.Age);
        client.WithIdentification(command.Identification);
        client.WithAddress(command.Address);
        client.WithPhone(command.Phone);
        client.WithStatus(command.Status);
        client.Touch();

        await clients.PublishUpsertedAsync(client, ct);

        return Result.Success(client.ToDto());
    }
}
