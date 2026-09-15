using System.Text.Json.Serialization;
using Identity.Domain.Errors;

namespace Identity.Api.Errors;

public sealed record ErrorResponse(string Code, string Detail, string TraceId)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IDictionary<string, string[]>? Errors { get; init; }

    public static ErrorResponse From(ErrorDefinition definition, HttpContext httpContext) =>
        new(definition.Code, definition.ResponseMessage, httpContext.TraceIdentifier);
}

public static class ErrorResponseWriter
{
    public static async Task WriteAsync(
        HttpContext httpContext,
        ErrorDefinition definition,
        CancellationToken cancellationToken = default)
    {
        if (httpContext.Response.HasStarted)
        {
            return;
        }

        httpContext.Response.StatusCode = definition.HttpStatus;

        await httpContext.Response.WriteAsJsonAsync(
            ErrorResponse.From(definition, httpContext),
            cancellationToken);
    }
}
