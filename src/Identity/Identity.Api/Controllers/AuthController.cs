using Identity.Api.Contracts;
using Identity.Api.Errors;
using Identity.Application.Abstractions.Messaging;
using Identity.Application.Commands.RevokeSessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[Route("api/v1/auth")]
[Produces("application/json")]
public sealed class AuthController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken ct)
    {
        var result = await mediator.SendAsync(request.ToCommand(), ct);

        if (result.IsFailure)
        {
            return Problem(result.Error);
        }

        var response = AuthResponse.From(result.Value);
        return Created($"/api/v1/users/{response.User.Id}", response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var result = await mediator.SendAsync(request.ToCommand(), ct);

        return result.IsSuccess ? Ok(AuthResponse.From(result.Value)) : Problem(result.Error);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Refresh(RefreshRequest request, CancellationToken ct)
    {
        var result = await mediator.SendAsync(request.ToCommand(), ct);

        return result.IsSuccess ? Ok(AuthResponse.From(result.Value)) : Problem(result.Error);
    }

    [Authorize]
    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Revoke(CancellationToken ct)
    {
        if (CurrentUserId is not { } userId)
        {
            return Unauthorized();
        }

        var result = await mediator.SendAsync(new RevokeSessionsCommand(userId), ct);

        return result.IsSuccess ? NoContent() : Problem(result.Error);
    }
}
