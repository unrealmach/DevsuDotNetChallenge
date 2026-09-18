using Account.Domain.Errors;

namespace Account.Infrastructure.Exceptions;

public sealed class InfrastructureException(
    ErrorDefinition error,
    string? context = null,
    Exception? innerException = null)
    : CodedException(error, context, innerException);
