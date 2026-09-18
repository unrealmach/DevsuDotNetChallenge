using FluentValidation;
using Identity.Domain.Entities;

namespace Identity.Application.Commands.PatchClient;

internal sealed class PatchClientCommandValidator : AbstractValidator<PatchClientCommand>
{
    public PatchClientCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();

        RuleFor(command => command.Age)
            .Must(age => int.TryParse(age, out var parsedAge) && parsedAge > 17)
            .WithMessage("La edad debe ser un numero entero mayor a 17.")
            .When(command => command.Age is not null);

        RuleFor(command => command.Status)
            .Must(status => ClientStatuses.All.Contains(status!))
            .WithMessage($"El estado del cliente debe ser '{ClientStatuses.Active}' o '{ClientStatuses.Inactive}'.")
            .When(command => command.Status is not null);
    }
}
