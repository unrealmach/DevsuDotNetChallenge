using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Commands.PatchClient;

internal sealed class PatchClientCommandHandler(IClientWriteService clients)
    : ICommandHandler<PatchClientCommand, ClientDto>
{
    public async Task<Result<ClientDto>> HandleAsync(PatchClientCommand command, CancellationToken ct = default)
    {
        var client = await clients.FindByIdAsync(command.Id, ct);

        if (client is null)
        {
            return Result.Failure<ClientDto>(ErrorCatalog.ClientNotFound, $"clientId={command.Id}");
        }

        if (command.Name is not null) client.WithName(command.Name);
        if (command.Gender is not null) client.WithGender(command.Gender);
        if (command.Age is not null) client.WithAge(command.Age);
        if (command.Identification is not null) client.WithIdentification(command.Identification);
        if (command.Address is not null) client.WithAddress(command.Address);
        if (command.Phone is not null) client.WithPhone(command.Phone);
        if (command.Status is not null) client.WithStatus(command.Status);
        if (command.Password is not null) client.WithPassword(clients.EncryptPassword(command.Password));

        client.Touch();

        await clients.PublishUpsertedAsync(client, ct);

        return Result.Success(client.ToDto());
    }
}
