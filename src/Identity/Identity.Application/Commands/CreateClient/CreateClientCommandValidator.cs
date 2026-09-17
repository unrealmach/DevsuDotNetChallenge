using FluentValidation;

namespace Identity.Application.Commands.CreateClient;

internal sealed class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(command => command.Age)
            .Must(age => int.TryParse(age, out var parsedAge) && parsedAge > 18)
            .WithMessage("La edad debe ser un numero entero mayor a 18.");
    }
}
