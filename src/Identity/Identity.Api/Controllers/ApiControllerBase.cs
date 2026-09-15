using System.Security.Claims;
using Identity.Api.Errors;
using Identity.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected Guid? CurrentUserId
    {
        get
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

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
