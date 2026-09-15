using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Services;
using Identity.Domain.Errors;

namespace Identity.Application.Queries.GetUserProfile;

internal sealed class GetUserProfileQueryHandler(IUserProfileService profiles)
    : IQueryHandler<GetUserProfileQuery, UserProfile>
{
    public async Task<Result<UserProfile>> HandleAsync(GetUserProfileQuery query, CancellationToken ct = default)
    {
        var profile = await profiles.GetAsync(query.UserId, ct);

        return profile is null
            ? Result.Failure<UserProfile>(ErrorCatalog.UserNotFound, $"userId={query.UserId}")
            : Result.Success(profile);
    }
}
