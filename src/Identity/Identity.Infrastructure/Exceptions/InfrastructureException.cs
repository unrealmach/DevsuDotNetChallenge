using Identity.Domain.Errors;

namespace Identity.Infrastructure.Exceptions;

public sealed class InfrastructureException(
    ErrorDefinition error,
    string? context = null,
    Exception? innerException = null)
    : CodedException(error, context, innerException);
