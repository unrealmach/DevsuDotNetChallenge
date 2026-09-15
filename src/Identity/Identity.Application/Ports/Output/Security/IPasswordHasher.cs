namespace Identity.Application.Ports.Output.Security;

internal interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string passwordHash);
}
