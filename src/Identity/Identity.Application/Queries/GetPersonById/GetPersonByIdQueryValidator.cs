using FluentValidation;

namespace Identity.Application.Queries.GetPersonById;

internal sealed class GetPersonByIdQueryValidator : AbstractValidator<GetPersonByIdQuery>
{
    public GetPersonByIdQueryValidator()
    {
        RuleFor(query => query.Id).NotEmpty();
    }
}
