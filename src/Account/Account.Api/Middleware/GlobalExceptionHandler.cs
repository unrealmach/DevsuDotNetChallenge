using System.Data.Common;
using Account.Api.Errors;
using Account.Domain.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace Account.Api.Middleware;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var definition = Resolve(exception);

        logger.Log(
            definition.Severity.ToLogLevel(),
            exception,
            "{ErrorCode} {ErrorLayer} {ErrorMessage} | {Method} {Path} | traceId={TraceId}",
            definition.Code,
            definition.Layer,
            definition.LogMessage,
            httpContext.Request.Method,
            httpContext.Request.Path,
            httpContext.TraceIdentifier);

        await ErrorResponseWriter.WriteAsync(httpContext, definition, cancellationToken);

        return true;
    }

    private static ErrorDefinition Resolve(Exception exception)
    {
        foreach (var current in Unwrap(exception))
        {
            var match = current switch
            {
                CodedException coded => coded.Error,
                BadHttpRequestException => ErrorCatalog.MalformedRequestBody,
                DbException => ErrorCatalog.DatabaseUnavailable,
                TimeoutException => ErrorCatalog.DatabaseUnavailable,
                _ => null
            };

            if (match is not null)
            {
                return match;
            }
        }

        return ErrorCatalog.Unhandled;
    }

    private static IEnumerable<Exception> Unwrap(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            yield return current;
        }
    }
}
