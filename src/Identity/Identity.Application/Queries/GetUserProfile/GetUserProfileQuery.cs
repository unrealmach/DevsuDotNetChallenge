using Identity.Application.Abstractions.Messaging;
using Identity.Application.Dtos;

namespace Identity.Application.Queries.GetUserProfile;

public sealed record GetUserProfileQuery(Guid UserId) : IQuery<UserProfile>;
