using FluentValidation;

namespace Identity.Application.Commands.CreatePerson;

internal sealed class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(command => command.Age)
            .Must(age => int.TryParse(age, out var parsedAge) && parsedAge > 18)
            .WithMessage("La edad debe ser un numero entero mayor a 18.");
    }
}
