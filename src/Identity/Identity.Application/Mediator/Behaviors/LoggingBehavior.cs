using System.Diagnostics;
using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Mediator.Behaviors;

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
        var name = typeof(TRequest).Name; //obtine nombre tipo del request
        var start = Stopwatch.GetTimestamp(); //inicia cronometro

        var response = await next(); //encapsula y llama a validationbehavior, espera a que lo interno termine por el await

        var elapsed = Stopwatch.GetElapsedTime(start).TotalMilliseconds;  //calcula cuanto tardo

        if (response is Result { IsFailure: true } failure) //respuesta con failer muestra logs
        {
            logger.LogWarning("{Request} rechazado en {Elapsed:F1}ms: {ErrorCode}", name, elapsed, failure.Error.Code);
        }
        else
        {
            logger.LogInformation("{Request} completado en {Elapsed:F1}ms", name, elapsed);
        }

        return response; // devuelve a afuera al mediator que lo pasa al controller
    }
}
