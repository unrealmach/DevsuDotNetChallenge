using FluentValidation;
using Identity.Application.Abstractions.Messaging;
using Identity.Application.Commands.CreatePerson;
using Identity.Application.Commands.DeletePerson;
using Identity.Application.Commands.UpdatePerson;
using Identity.Application.Common;
using Identity.Application.Dtos;
using Identity.Application.Mediator.Behaviors;
using Identity.Application.Queries.GetPersonById;
using Identity.Application.Queries.GetPersons;
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

        services.AddScoped<IPersonReadService, PersonReadService>();
        services.AddScoped<IPersonValidationService, PersonValidationService>();
        services.AddScoped<IPersonWriteService, PersonWriteService>();

        //registro explicito de validators
        services
            .AddScoped<IValidator<CreatePersonCommand>, CreatePersonCommandValidator>()
            .AddScoped<IValidator<UpdatePersonCommand>, UpdatePersonCommandValidator>()
            .AddScoped<IValidator<DeletePersonCommand>, DeletePersonCommandValidator>()
            .AddScoped<IValidator<GetPersonByIdQuery>, GetPersonByIdQueryValidator>();
        
        //registro explicito de handlers
        services
            .AddScoped<IRequestHandler<CreatePersonCommand, Result<PersonDto>>,
                CreatePersonCommandHandler>()
            .AddScoped<IRequestHandler<UpdatePersonCommand, Result<PersonDto>>,
                UpdatePersonCommandHandler>()
            .AddScoped<IRequestHandler<DeletePersonCommand, Result>,
                DeletePersonCommandHandler>()
            .AddScoped<IRequestHandler<GetPersonByIdQuery, Result<PersonDto>>,
                GetPersonByIdQueryHandler>()
            .AddScoped<IRequestHandler<GetPersonsQuery, Result<IReadOnlyList<PersonDto>>>,
                GetPersonsQueryHandler>();

        return services;
    }
}
