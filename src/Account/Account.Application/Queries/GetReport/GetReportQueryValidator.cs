using FluentValidation;

namespace Account.Application.Queries.GetReport;

internal sealed class GetReportQueryValidator : AbstractValidator<GetReportQuery>
{
    public GetReportQueryValidator()
    {
        RuleFor(query => query.To)
            .GreaterThanOrEqualTo(query => query.From)
            .WithMessage("La fecha 'hasta' no puede ser anterior a la fecha 'desde'.");
    }
}
