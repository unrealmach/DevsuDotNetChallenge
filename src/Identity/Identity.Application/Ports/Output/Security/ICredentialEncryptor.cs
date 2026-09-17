namespace Identity.Application.Ports.Output.Security;

internal interface ICredentialEncryptor
{
    string Encrypt(string plainText);

    string Decrypt(string cipherText);
}
