using FluentValidation;

namespace Identity.Application.Commands.UpdateClient;

internal sealed class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.Age)
            .Must(age => int.TryParse(age, out var parsedAge) && parsedAge > 18)
            .WithMessage("La edad debe ser un numero entero mayor a 18.");
    }
}
