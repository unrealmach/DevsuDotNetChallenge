namespace Account.Domain.Errors;

public abstract class CodedException : Exception
{
    protected CodedException(ErrorDefinition error, string? context = null, Exception? innerException = null)
        : base(error.Describe(context), innerException)
    {
        Error = error;
        Context = context;
    }

    public ErrorDefinition Error { get; }

    public string? Context { get; }
}