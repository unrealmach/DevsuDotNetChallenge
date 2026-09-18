using Account.Domain.Entities;
using FluentValidation;

namespace Account.Application.Commands.CreateAccount;

internal sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(command => command.ClientId).NotEmpty();

        RuleFor(command => command.Type)
            .NotEmpty()
            .Must(type => AccountTypes.All.Contains(type))
            .WithMessage($"El tipo de cuenta debe ser '{AccountTypes.Checking}' o '{AccountTypes.Savings}'.");

        RuleFor(command => command.Status)
            .NotEmpty()
            .Must(status => AccountStatuses.All.Contains(status))
            .WithMessage($"El estado de la cuenta debe ser '{AccountStatuses.Active}' o '{AccountStatuses.Inactive}'.");
    }
}
