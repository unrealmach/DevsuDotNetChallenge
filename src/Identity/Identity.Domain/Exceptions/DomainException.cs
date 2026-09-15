using Identity.Domain.Errors;

namespace Identity.Domain.Exceptions;

public sealed class DomainException(ErrorDefinition error, string? context = null, Exception? innerException = null)
    : CodedException(error, context, innerException);
