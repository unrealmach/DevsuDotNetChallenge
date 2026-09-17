using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Application.Ports.Output.Write;

namespace Identity.Application.Mediator.Behaviors;

internal sealed class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private static readonly bool IsCommand = // TRequest es un ICommand (escribe o lee) 
        typeof(ICommand).IsAssignableFrom(typeof(TRequest))
        || Array.Exists(
            typeof(TRequest).GetInterfaces(),
            contract => contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(ICommand<>)); // match contra la interfaz command

    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var response = await next(); //llama al handler

        if (IsCommand && response is Result { IsSuccess: true }) // si es un Trequest Writer y comand es exitoso
        {
            await unitOfWork.SaveChangesAsync(ct); //guarda
        }

        return response;
    }
}
