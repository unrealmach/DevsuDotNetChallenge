using System.Collections.Concurrent;
using Account.Application.Abstractions.Messaging;
using Account.Application.Exceptions;
using Account.Domain.Errors;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Application.Mediator;

internal sealed class Mediator(IServiceProvider provider) : IMediator
{
    private static readonly ConcurrentDictionary<Type, object> Wrappers = new();

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        var wrapper = (RequestWrapper<TResponse>)Wrappers.GetOrAdd(
            request.GetType(),
            requestType => Activator.CreateInstance(
                typeof(RequestWrapper<,>).MakeGenericType(requestType, typeof(TResponse)))!);

        return wrapper.HandleAsync(request, provider, ct);
    }

    private abstract class RequestWrapper<TResponse>
    {
        public abstract Task<TResponse> HandleAsync(
            IRequest<TResponse> request,
            IServiceProvider provider,
            CancellationToken ct);
    }

    private sealed class RequestWrapper<TRequest, TResponse> : RequestWrapper<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override Task<TResponse> HandleAsync(
            IRequest<TResponse> request,
            IServiceProvider provider,
            CancellationToken ct)
        {
            var handler = provider.GetService<IRequestHandler<TRequest, TResponse>>()
                          ?? throw new UseCaseException(ErrorCatalog.HandlerNotRegistered, typeof(TRequest).Name);

            var message = (TRequest)request;
            RequestHandlerDelegate<TResponse> pipeline = () => handler.HandleAsync(message, ct);

            foreach (var behavior in provider.GetServices<IPipelineBehavior<TRequest, TResponse>>().Reverse())
            {
                var next = pipeline;
                pipeline = () => behavior.HandleAsync(message, next, ct);
            }

            return pipeline();
        }
    }
}
