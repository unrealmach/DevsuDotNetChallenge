using FluentValidation;
using Identity.Application.Abstractions.Messaging;
using Identity.Application.Commands.CreateClient;
using Identity.Application.Commands.DeleteClient;
using Identity.Application.Commands.PatchClient;
using Identity.Application.Commands.UpdateClient;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Mediator.Behaviors;
using Identity.Application.Queries.GetClients;
using Identity.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator.Mediator>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

        // registro explicito en services
        services.AddScoped<IClientValidationService, ClientValidationService>();
        services.AddScoped<IClientWriteService, ClientWriteService>();
        services.AddScoped<IClientReadService, ClientReadService>();

        //registro explicito de validators
        services
            .AddScoped<IValidator<CreateClientCommand>, CreateClientCommandValidator>()
            .AddScoped<IValidator<UpdateClientCommand>, UpdateClientCommandValidator>()
            .AddScoped<IValidator<PatchClientCommand>, PatchClientCommandValidator>()
            .AddScoped<IValidator<DeleteClientCommand>, DeleteClientCommandValidator>();

        //registro explicito de handlers
        services
            .AddScoped<IRequestHandler<CreateClientCommand, Result<ClientDto>>,
                CreateClientCommandHandler>()
            .AddScoped<IRequestHandler<UpdateClientCommand, Result<ClientDto>>,
                UpdateClientCommandHandler>()
            .AddScoped<IRequestHandler<PatchClientCommand, Result<ClientDto>>,
                PatchClientCommandHandler>()
            .AddScoped<IRequestHandler<DeleteClientCommand, Result>,
                DeleteClientCommandHandler>()
            .AddScoped<IRequestHandler<GetClientsQuery, Result<IReadOnlyList<ClientDto>>>,
                GetClientsQueryHandler>();

        return services;
    }
}
