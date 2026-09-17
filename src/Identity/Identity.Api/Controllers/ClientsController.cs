using Identity.Api.Contracts;
using Identity.Api.Errors;
using Identity.Application.Abstractions.Messaging;
using Identity.Application.Commands.DeleteClient;
using Identity.Application.Queries.GetClients;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[Route("api/v1/clients")]
[Produces("application/json")]
public sealed class ClientsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ClientResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ClientResponse>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.SendAsync(new GetClientsQuery(), ct);

        return Ok(result.Value.Select(ClientResponse.From));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClientResponse>> Create(CreateClientRequest request, CancellationToken ct)
    {
        var result = await mediator.SendAsync(request.ToCommand(), ct);

        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        var response = ClientResponse.From(result.Value);
        return Created($"/api/v1/clients/{response.Id}", response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientResponse>> Update(Guid id, UpdateClientRequest request, CancellationToken ct)
    {
        var result = await mediator.SendAsync(request.ToCommand(id), ct);

        return result.IsSuccess ? Ok(ClientResponse.From(result.Value)) : Problem(result.Error);
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientResponse>> Patch(Guid id, PatchClientRequest request, CancellationToken ct)
    {
        var result = await mediator.SendAsync(request.ToCommand(id), ct);

        return result.IsSuccess ? Ok(ClientResponse.From(result.Value)) : Problem(result.Error);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.SendAsync(new DeleteClientCommand(id), ct);

        return result.IsSuccess ? NoContent() : Problem(result.Error);
    }
}
