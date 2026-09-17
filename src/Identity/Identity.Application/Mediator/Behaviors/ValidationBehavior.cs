using System.Reflection;
using FluentValidation;
using Identity.Application.Abstractions.Messaging;
using Identity.Application.Common;
using Identity.Domain.Errors;

namespace Identity.Application.Mediator.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) //constructor con todos los validators registrados para command/query
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (!validators.Any())
        {
            return await next(); // pasa a unit of work
        }

        var failures = new List<string>();

        foreach (var validator in validators) //itera sobre la lista de validadores
        {
            var result = await validator.ValidateAsync(request, ct); //ejecuta 
            failures.AddRange(result.Errors.Select(failure => $"{failure.PropertyName}: {failure.ErrorMessage}")); //junta en lista
        }

        if (failures.Count == 0)
        {
            return await next(); // pasa a unit of work
        }

        var error = Error.From(ErrorCatalog.RequestValidationFailed, string.Join(" | ", failures));

        return CreateFailureResponse(error); //crea una respuesta tipo failure
    }

    private static TResponse CreateFailureResponse(Error error)
    {
        if (typeof(TResponse) == typeof(Result)) // si es un result sin generico proveniente del handler
        {
            return (TResponse)(object)Result.Failure(error); //crea un result con error
        }

        var valueType = typeof(TResponse).GetGenericArguments()[0]; // al ser un result la obtiene y la devuelve
        var failureMethod = typeof(Result)
            .GetMethod(nameof(Result.Failure), 1, BindingFlags.Public | BindingFlags.Static, null, [typeof(Error)], null)!
            .MakeGenericMethod(valueType);

        return (TResponse)failureMethod.Invoke(null, [error])!;
    }
}
