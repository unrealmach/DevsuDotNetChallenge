using Account.Api.Errors;
using Account.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult Problem(Error error)
    {
        HttpContext.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger(GetType())
            .Log(
                error.Severity.ToLogLevel(),
                "{ErrorCode} {ErrorLayer} {ErrorMessage} | {Method} {Path} | traceId={TraceId}",
                error.Code,
                error.Layer,
                error.LogMessage,
                HttpContext.Request.Method,
                HttpContext.Request.Path,
                HttpContext.TraceIdentifier);

        return StatusCode(error.HttpStatus, ErrorResponse.From(error.Definition, HttpContext));
    }
}
