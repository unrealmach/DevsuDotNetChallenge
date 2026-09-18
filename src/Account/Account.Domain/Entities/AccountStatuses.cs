namespace Account.Domain.Entities;

// Unica fuente de verdad de que valores acepta Account.Status.
public static class AccountStatuses
{
    public const string Active = "ACTIVE";
    public const string Inactive = "INACTIVE";

    public static readonly IReadOnlyCollection<string> All = [Active, Inactive];
}
