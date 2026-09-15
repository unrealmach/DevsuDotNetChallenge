namespace Identity.Domain;

public static class Roles
{
    public const string Admin = "admin";
    public const string User = "user";

    public static bool IsValid(string role) => role is Admin or User;
}
