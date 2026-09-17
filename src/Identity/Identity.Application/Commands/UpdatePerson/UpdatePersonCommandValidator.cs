using FluentValidation;

namespace Identity.Application.Commands.UpdatePerson;

internal sealed class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.Age)
            .Must(age => int.TryParse(age, out var parsedAge) && parsedAge > 18)
            .WithMessage("La edad debe ser un numero entero mayor a 18.");
    }
}
