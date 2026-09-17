using FluentValidation;

namespace Identity.Application.Commands.DeleteClient;

internal sealed class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
    }
}
