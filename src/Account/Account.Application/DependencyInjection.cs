using Account.Application.Abstractions.Messaging;
using Account.Application.Commands.CreateAccount;
using Account.Application.Commands.RegisterMovement;
using Account.Application.Common;
using Account.Application.Dtos;
using Account.Application.Mediator.Behaviors;
using Account.Application.Queries.GetAccounts;
using Account.Application.Queries.GetClients;
using Account.Application.Queries.GetMovements;
using Account.Application.Queries.GetReport;
using Account.Application.Services;
using FluentValidation;
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

        // registro explicito en services
        services.AddScoped<IAccountReadService, AccountReadService>();
        services.AddScoped<IAccountWriteService, AccountWriteService>();
        services.AddScoped<IClientReadService, ClientReadService>();
        services.AddScoped<IMovementReadService, MovementReadService>();
        services.AddScoped<IMovementService, MovementService>();

        //registro explicito de validators
        services
            .AddScoped<IValidator<CreateAccountCommand>, CreateAccountCommandValidator>()
            .AddScoped<IValidator<RegisterMovementCommand>, RegisterMovementCommandValidator>()
            .AddScoped<IValidator<GetReportQuery>, GetReportQueryValidator>();

        //registro explicito de handlers
        services
            .AddScoped<IRequestHandler<CreateAccountCommand, Result<AccountDto>>,
                CreateAccountCommandHandler>()
            .AddScoped<IRequestHandler<RegisterMovementCommand, Result<MovementDto>>,
                RegisterMovementCommandHandler>()
            .AddScoped<IRequestHandler<GetAccountsQuery, Result<IReadOnlyList<AccountDto>>>,
                GetAccountsQueryHandler>()
            .AddScoped<IRequestHandler<GetClientsQuery, Result<IReadOnlyList<ClientDto>>>,
                GetClientsQueryHandler>()
            .AddScoped<IRequestHandler<GetMovementsQuery, Result<IReadOnlyList<MovementDto>>>,
                GetMovementsQueryHandler>()
            .AddScoped<IRequestHandler<GetReportQuery, Result<IReadOnlyList<MovementDto>>>,
                GetReportQueryHandler>();

        return services;
    }
}
