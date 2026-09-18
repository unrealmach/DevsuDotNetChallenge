using Account.Application.Abstractions.Messaging;
using Account.Application.Mediator.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator.Mediator>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        // Cuando agregues el primer Command/Query de Account, sus registros van aca
        // (validators, servicios de Read/Validate/Write y handlers), igual que en Identity.

        return services;
    }
}
