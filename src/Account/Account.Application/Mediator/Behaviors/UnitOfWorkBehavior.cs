using Account.Application.Abstractions.Messaging;
using Account.Application.Common;
using Account.Application.Ports.Output.Write;

namespace Account.Application.Mediator.Behaviors;

internal sealed class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private static readonly bool IsCommand =
        typeof(ICommand).IsAssignableFrom(typeof(TRequest))
        || Array.Exists(
            typeof(TRequest).GetInterfaces(),
            contract => contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(ICommand<>));

    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var response = await next();

        if (IsCommand && response is Result { IsSuccess: true })
        {
            await unitOfWork.SaveChangesAsync(ct);
        }

        return response;
    }
}
