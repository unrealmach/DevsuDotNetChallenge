using Account.Api.Contracts;
using Account.Application.Abstractions.Messaging;
using Account.Application.Queries.GetClients;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers;

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
}
