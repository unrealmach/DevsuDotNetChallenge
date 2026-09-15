using Identity.Api.Contracts;
using Identity.Api.Errors;
using Identity.Application.Abstractions.Messaging;
using Identity.Application.Queries.GetUserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[Authorize]
[Route("api/v1/users")]
[Produces("application/json")]
public sealed class UsersController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> Me(CancellationToken ct)
    {
        if (CurrentUserId is not { } userId)
        {
            return Unauthorized();
        }

        var result = await mediator.SendAsync(new GetUserProfileQuery(userId), ct);

        return result.IsSuccess ? Ok(UserResponse.From(result.Value)) : Problem(result.Error);
    }
}
