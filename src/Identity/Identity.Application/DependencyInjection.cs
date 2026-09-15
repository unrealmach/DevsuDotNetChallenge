using Identity.Application.Abstractions.Messaging;
using Identity.Application.Commands.AuthenticateUser;
using Identity.Application.Commands.RefreshSession;
using Identity.Application.Commands.RegisterUser;
using Identity.Application.Commands.RevokeSessions;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Mediator.Behaviors;
using Identity.Application.Queries.GetUserProfile;
using Identity.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator.Mediator>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IUserProfileService, UserProfileService>();

        services
            .AddScoped<IRequestHandler<RegisterUserCommand, Result<AuthenticationResult>>,
                RegisterUserCommandHandler>()
            .AddScoped<IRequestHandler<AuthenticateUserCommand, Result<AuthenticationResult>>,
                AuthenticateUserCommandHandler>()
            .AddScoped<IRequestHandler<RefreshSessionCommand, Result<AuthenticationResult>>,
                RefreshSessionCommandHandler>()
            .AddScoped<IRequestHandler<RevokeSessionsCommand, Result>,
                RevokeSessionsCommandHandler>()
            .AddScoped<IRequestHandler<GetUserProfileQuery, Result<UserProfile>>,
                GetUserProfileQueryHandler>();

        return services;
    }
}
