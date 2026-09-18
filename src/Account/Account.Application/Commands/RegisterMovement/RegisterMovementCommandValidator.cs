using FluentValidation;

namespace Account.Application.Commands.RegisterMovement;

internal sealed class RegisterMovementCommandValidator : AbstractValidator<RegisterMovementCommand>
{
    public RegisterMovementCommandValidator()
    {
        RuleFor(command => command.AccountId).NotEmpty();

        RuleFor(command => command.Value)
            .NotEqual(0)
            .WithMessage("El valor del movimiento no puede ser cero.");
    }
}
