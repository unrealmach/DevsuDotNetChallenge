using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Commands.CreatePerson;

public sealed record CreatePersonCommand(
    string Name,
    string Gender,
    string Age,
    string Identification,
    string Address,
    string Phone) : ICommand<PersonDto>;
