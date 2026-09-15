namespace Identity.Domain.Errors;

public sealed record ErrorDefinition(
    string Code,
    ErrorLayer Layer,
    ErrorSeverity Severity,
    ErrorExposure Exposure,
    int HttpStatus,
    string LogMessage,
    string? ClientMessage = null)
{
    public static readonly ErrorDefinition None = new(
        "IDN-000",
        ErrorLayer.Application,
        ErrorSeverity.Information,
        ErrorExposure.LogOnly,
        200,
        "Sin error");

    public string ResponseMessage =>
        Exposure is ErrorExposure.Response && ClientMessage is not null
            ? ClientMessage
            : ErrorCatalog.GenericResponseMessage;

    public string Describe(string? context = null) =>
        context is null
            ? $"[{Code}] [{Layer}] {LogMessage}"
            : $"[{Code}] [{Layer}] {LogMessage}: {context}";
}
