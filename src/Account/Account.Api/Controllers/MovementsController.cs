using Account.Api.Contracts;
using Account.Api.Errors;
using Account.Application.Abstractions.Messaging;
using Account.Application.Queries.GetMovements;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers;

[Produces("application/json")]
public sealed class MovementsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("api/v1/movements")]
    [ProducesResponseType(typeof(IReadOnlyList<MovementResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MovementResponse>>> GetAll(
        [FromQuery] Guid? accountId,
        CancellationToken ct)
    {
        var result = await mediator.SendAsync(new GetMovementsQuery(accountId), ct);

        return Ok(result.Value.Select(MovementResponse.From));
    }

    [HttpPost("api/v1/accounts/{accountId:guid}/movements")]
    [ProducesResponseType(typeof(MovementResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status423Locked)]
    public async Task<ActionResult<MovementResponse>> Register(
        Guid accountId,
        RegisterMovementRequest request,
        CancellationToken ct)
    {
        var result = await mediator.SendAsync(request.ToCommand(accountId), ct);

        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        var response = MovementResponse.From(result.Value);
        return Created($"/api/v1/movements/{response.Id}", response);
    }
}
