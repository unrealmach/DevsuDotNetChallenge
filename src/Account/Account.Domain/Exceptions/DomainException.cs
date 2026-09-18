using Account.Domain.Errors;

namespace Account.Domain.Exceptions;

public sealed class DomainException(ErrorDefinition error, string? context = null, Exception? innerException = null)
    : CodedException(error, context, innerException);