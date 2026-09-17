using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;

namespace Identity.Application.Commands.CreatePerson;

internal sealed class CreatePersonCommandHandler(IPersonWriteService persons)
    : ICommandHandler<CreatePersonCommand, PersonDto>
{
    public async Task<Result<PersonDto>> HandleAsync(CreatePersonCommand command, CancellationToken ct = default)
    {
        var person = await persons.CreateAsync(
            command.Name,
            command.Gender,
            command.Age,
            command.Identification,
            command.Address,
            command.Phone,
            ct);

        return Result.Success(person.ToDto());
    }
}
