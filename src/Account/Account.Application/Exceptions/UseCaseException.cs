using Account.Domain.Errors;

namespace Account.Application.Exceptions;

public sealed class UseCaseException(ErrorDefinition error, string? context = null, Exception? innerException = null)
    : CodedException(error, context, innerException);
