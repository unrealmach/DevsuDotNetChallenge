using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Commands.UpdatePerson;

internal sealed class UpdatePersonCommandHandler(IPersonWriteService persons)
    : ICommandHandler<UpdatePersonCommand, PersonDto>
{
    public async Task<Result<PersonDto>> HandleAsync(UpdatePersonCommand command, CancellationToken ct = default)
    {
        var person = await persons.FindByIdAsync(command.Id, ct);

        if (person is null)
        {
            return Result.Failure<PersonDto>(ErrorCatalog.PersonNotFound, $"personId={command.Id}");
        }

        person
            .WithName(command.Name)
            .WithGender(command.Gender)
            .WithAge(command.Age)
            .WithIdentification(command.Identification)
            .WithAddress(command.Address)
            .WithPhone(command.Phone)
            .Touch();

        return Result.Success(person.ToDto());
    }
}
