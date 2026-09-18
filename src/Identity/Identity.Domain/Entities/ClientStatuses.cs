namespace Identity.Domain.Entities;

// Unica fuente de verdad de que valores acepta Client.Status.
public static class ClientStatuses
{
    public const string Active = "ACTIVE";
    public const string Inactive = "INACTIVE";

    public static readonly IReadOnlyCollection<string> All = [Active, Inactive];
}
