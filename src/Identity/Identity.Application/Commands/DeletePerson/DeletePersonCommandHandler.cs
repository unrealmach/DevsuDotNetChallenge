using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Commands.DeletePerson;

internal sealed class DeletePersonCommandHandler(IPersonValidationService validation, IPersonWriteService persons)
    : ICommandHandler<DeletePersonCommand>
{
    public async Task<Result> HandleAsync(DeletePersonCommand command, CancellationToken ct = default)
    {
        if (!await validation.ExistsAsync(command.Id, ct))
        {
            return Result.Failure(ErrorCatalog.PersonNotFound, $"personId={command.Id}");
        }

        var person = await persons.FindByIdAsync(command.Id, ct);

        if (person is null)
        {
            return Result.Failure(ErrorCatalog.PersonNotFound, $"personId={command.Id}");
        }

        persons.Remove(person);

        return Result.Success();
    }
}
