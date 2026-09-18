using FluentValidation;
using Identity.Domain.Entities;

namespace Identity.Application.Commands.CreateClient;

internal sealed class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(command => command.Age)
            .Must(age => int.TryParse(age, out var parsedAge) && parsedAge > 17)
            .WithMessage("La edad debe ser un numero entero mayor a 17.");

        RuleFor(command => command.Status)
            .Must(status => ClientStatuses.All.Contains(status))
            .WithMessage($"El estado del cliente debe ser '{ClientStatuses.Active}' o '{ClientStatuses.Inactive}'.");
    }
}
