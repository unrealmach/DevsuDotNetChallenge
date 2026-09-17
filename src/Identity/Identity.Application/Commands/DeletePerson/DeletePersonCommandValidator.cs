using FluentValidation;

namespace Identity.Application.Commands.DeletePerson;

internal sealed class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
    }
}
