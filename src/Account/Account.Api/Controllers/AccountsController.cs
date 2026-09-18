using Account.Api.Contracts;
using Account.Api.Errors;
using Account.Application.Abstractions.Messaging;
using Account.Application.Queries.GetAccounts;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers;

[Route("api/v1/accounts")]
[Produces("application/json")]
public sealed class AccountsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AccountResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AccountResponse>>> GetAll(
        [FromQuery] Guid? clientId,
        CancellationToken ct)
    {
        var result = await mediator.SendAsync(new GetAccountsQuery(clientId), ct);

        return Ok(result.Value.Select(AccountResponse.From));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccountResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.SendAsync(new GetAccountsQuery(null), ct);
        var account = result.Value.FirstOrDefault(a => a.Id == id);

        return account is null ? NotFound() : Ok(AccountResponse.From(account));
    }

    [HttpPost]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccountResponse>> Create(CreateAccountRequest request, CancellationToken ct)
    {
        var result = await mediator.SendAsync(request.ToCommand(), ct);

        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        var response = AccountResponse.From(result.Value);
        return Created($"/api/v1/accounts/{response.Id}", response);
    }
}
