using Identity.Domain.Errors;

namespace Identity.Api.Errors;

public static class ErrorSeverityExtensions
{
    public static LogLevel ToLogLevel(this ErrorSeverity severity) => severity switch
    {
        ErrorSeverity.Information => LogLevel.Information,
        ErrorSeverity.Warning => LogLevel.Warning,
        ErrorSeverity.Error => LogLevel.Error,
        ErrorSeverity.Critical => LogLevel.Critical,
        _ => LogLevel.Error
    };
}
