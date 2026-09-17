using Identity.Api.Contracts;
using Identity.Api.Errors;
using Identity.Application.Abstractions.Messaging;
using Identity.Application.Commands.DeletePerson;
using Identity.Application.Queries.GetPersonById;
using Identity.Application.Queries.GetPersons;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[Route("api/v1/persons")]
[Produces("application/json")]
public sealed class PersonsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PersonResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PersonResponse>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.SendAsync(new GetPersonsQuery(), ct);

        return Ok(result.Value.Select(PersonResponse.From));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonResponse>> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.SendAsync(new GetPersonByIdQuery(id), ct);

        return result.IsSuccess ? Ok(PersonResponse.From(result.Value)) : Problem(result.Error);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonResponse>> Create(CreatePersonRequest request, CancellationToken ct)
    {
        var result = await mediator.SendAsync(request.ToCommand(), ct);

        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        var response = PersonResponse.From(result.Value);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonResponse>> Update(
        Guid id,
        UpdatePersonRequest request,
        CancellationToken ct)
    {
        var result = await mediator.SendAsync(request.ToCommand(id), ct);

        return result.IsSuccess ? Ok(PersonResponse.From(result.Value)) : Problem(result.Error);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.SendAsync(new DeletePersonCommand(id), ct);

        return result.IsSuccess ? NoContent() : Problem(result.Error);
    }
}
