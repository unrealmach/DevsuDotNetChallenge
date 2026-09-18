using Account.Api.Contracts;
using Account.Api.Errors;
using Account.Application.Abstractions.Messaging;
using Account.Application.Queries.GetReport;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers;

[Route("api/v1/reportes")]
[Produces("application/json")]
public sealed class ReportsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MovementResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<MovementResponse>>> Get(
        [FromQuery] DateTime desde,
        [FromQuery] DateTime hasta,
        [FromQuery] Guid? clienteId,
        CancellationToken ct)
    {
        var from = DateTime.SpecifyKind(desde, DateTimeKind.Utc);
        var to = DateTime.SpecifyKind(hasta, DateTimeKind.Utc);

        var result = await mediator.SendAsync(new GetReportQuery(from, to, clienteId), ct);

        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        return Ok(result.Value.Select(MovementResponse.From));
    }
}
