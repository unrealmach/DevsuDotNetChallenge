using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Commands.UpdatePerson;

public sealed record UpdatePersonCommand(
    Guid Id,
    string Name,
    string Gender,
    string Age,
    string Identification,
    string Address,
    string Phone) : ICommand<PersonDto>;
