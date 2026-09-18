namespace Account.Domain.Entities;

// Unica fuente de verdad de que valores acepta Account.
public static class AccountTypes
{
    public const string Checking = "CHECKING";
    public const string Savings = "SAVINGS";

    public static readonly IReadOnlyCollection<string> All = [Checking, Savings];
}
