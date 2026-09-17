using FluentValidation;

namespace Identity.Application.Commands.PatchClient;

internal sealed class PatchClientCommandValidator : AbstractValidator<PatchClientCommand>
{
    public PatchClientCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();

        RuleFor(command => command.Age)
            .Must(age => int.TryParse(age, out var parsedAge) && parsedAge > 18)
            .WithMessage("La edad debe ser un numero entero mayor a 18.")
            .When(command => command.Age is not null);
    }
}
