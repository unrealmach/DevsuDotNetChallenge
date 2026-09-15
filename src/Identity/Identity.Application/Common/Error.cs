using Identity.Domain.Errors;

namespace Identity.Application.Common;

public sealed record Error(ErrorDefinition Definition, string? Context = null)
{
    public static readonly Error None = new(ErrorDefinition.None);

    public string Code => Definition.Code;

    public ErrorLayer Layer => Definition.Layer;

    public ErrorSeverity Severity => Definition.Severity;

    public int HttpStatus => Definition.HttpStatus;

    public string LogMessage => Definition.Describe(Context);

    public string ResponseMessage => Definition.ResponseMessage;

    public static Error From(ErrorDefinition definition, string? context = null) => new(definition, context);
}
