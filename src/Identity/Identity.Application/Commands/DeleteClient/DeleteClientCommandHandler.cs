using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Commands.DeleteClient;

internal sealed class DeleteClientCommandHandler(IClientValidationService validation, IClientWriteService clients)
    : ICommandHandler<DeleteClientCommand>
{
    public async Task<Result> HandleAsync(DeleteClientCommand command, CancellationToken ct = default)
    {
        if (!await validation.ExistsAsync(command.Id, ct))
        {
            return Result.Failure(ErrorCatalog.ClientNotFound, $"clientId={command.Id}");
        }

        var client = await clients.FindByIdAsync(command.Id, ct);

        if (client is null)
        {
            return Result.Failure(ErrorCatalog.ClientNotFound, $"clientId={command.Id}");
        }

        clients.Remove(client);

        return Result.Success();
    }
}
