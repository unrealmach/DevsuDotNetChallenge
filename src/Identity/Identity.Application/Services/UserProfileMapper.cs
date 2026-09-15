using Identity.Application.Dtos;
using Identity.Domain.Entities;

namespace Identity.Application.Services;

internal static class UserProfileMapper
{
    public static UserProfile ToProfile(this User user) =>
        new(user.Id, user.Email, user.FullName, user.Role, user.CreatedAtUtc);
}
