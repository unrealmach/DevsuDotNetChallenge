using System.Security.Cryptography;
using System.Text;
using Identity.Application.Ports.Output.Security;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Adapters.Security;

internal sealed class AesGcmCredentialEncryptorAdapter : ICredentialEncryptor
{
    private const int NonceSize = 12;
    private const int TagSize = 16;

    private readonly byte[] _key;

    public AesGcmCredentialEncryptorAdapter(IOptions<AesGcmOptions> options)
    {
        _key = Convert.FromBase64String(options.Value.Key);
    }

    public string Encrypt(string plainText)
    {
        var nonce = RandomNumberGenerator.GetBytes(NonceSize);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = new byte[plainBytes.Length];
        var tag = new byte[TagSize];

        using (var aesGcm = new AesGcm(_key, TagSize))
        {
            aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);
        }

        var payload = new byte[NonceSize + TagSize + cipherBytes.Length];
        Buffer.BlockCopy(nonce, 0, payload, 0, NonceSize);
        Buffer.BlockCopy(tag, 0, payload, NonceSize, TagSize);
        Buffer.BlockCopy(cipherBytes, 0, payload, NonceSize + TagSize, cipherBytes.Length);

        return Convert.ToBase64String(payload);
    }

    public string Decrypt(string cipherText)
    {
        var payload = Convert.FromBase64String(cipherText);

        var nonce = payload.AsSpan(0, NonceSize);
        var tag = payload.AsSpan(NonceSize, TagSize);
        var cipherBytes = payload.AsSpan(NonceSize + TagSize);

        var plainBytes = new byte[cipherBytes.Length];

        using (var aesGcm = new AesGcm(_key, TagSize))
        {
            aesGcm.Decrypt(nonce, cipherBytes, tag, plainBytes);
        }

        return Encoding.UTF8.GetString(plainBytes);
    }
}
