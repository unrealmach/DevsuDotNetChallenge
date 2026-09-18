using System.Diagnostics;
using Account.Application.Abstractions.Messaging;
using Account.Application.Common;
using Microsoft.Extensions.Logging;

namespace Account.Application.Mediator.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var name = typeof(TRequest).Name;
        var start = Stopwatch.GetTimestamp();

        var response = await next();

        var elapsed = Stopwatch.GetElapsedTime(start).TotalMilliseconds;

        if (response is Result { IsFailure: true } failure)
        {
            logger.LogWarning("{Request} rechazado en {Elapsed:F1}ms: {ErrorCode}", name, elapsed, failure.Error.Code);
        }
        else
        {
            logger.LogInformation("{Request} completado en {Elapsed:F1}ms", name, elapsed);
        }

        return response;
    }
}
