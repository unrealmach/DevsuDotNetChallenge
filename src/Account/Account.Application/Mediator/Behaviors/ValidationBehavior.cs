using System.Reflection;
using FluentValidation;
using Account.Application.Abstractions.Messaging;
using Account.Application.Common;
using Account.Domain.Errors;

namespace Account.Application.Mediator.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
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
            return await next();
        }

        var failures = new List<string>();

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(request, ct);
            failures.AddRange(result.Errors.Select(failure => $"{failure.PropertyName}: {failure.ErrorMessage}"));
        }

        if (failures.Count == 0)
        {
            return await next();
        }

        var error = Error.From(ErrorCatalog.RequestValidationFailed, string.Join(" | ", failures));

        return CreateFailureResponse(error);
    }

    private static TResponse CreateFailureResponse(Error error)
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(error);
        }

        var valueType = typeof(TResponse).GetGenericArguments()[0];
        var failureMethod = typeof(Result)
            .GetMethod(nameof(Result.Failure), 1, BindingFlags.Public | BindingFlags.Static, null, [typeof(Error)], null)!
            .MakeGenericMethod(valueType);

        return (TResponse)failureMethod.Invoke(null, [error])!;
    }
}
