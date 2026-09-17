using FluentAssertions;
using Identity.Infrastructure.Adapters.Security;
using Microsoft.Extensions.Options;

namespace Identity.UnitTests.Infrastructure.Security;

public class AesGcmCredentialEncryptorAdapterTests
{
    private readonly AesGcmCredentialEncryptorAdapter _encryptor = new(
        Options.Create(new AesGcmOptions { Key = "AL2m6FOb1arv3rfzfay6FIf30rjSAdG+aeIEIxiC69w=" }));

    [Fact]
    public void Encrypt_ShouldProduceCipherText_DifferentFromThePlainText()
    {
        // Arrange
        const string plainText = "MiClaveSecreta1";

        // Act
        var cipherText = _encryptor.Encrypt(plainText);

        // Assert
        cipherText.Should().NotBe(plainText);
    }

    [Fact]
    public void Decrypt_ShouldReturnTheOriginalPlainText_AfterEncrypt()
    {
        // Arrange
        const string plainText = "MiClaveSecreta1";
        var cipherText = _encryptor.Encrypt(plainText);

        // Act
        var decrypted = _encryptor.Decrypt(cipherText);

        // Assert
        decrypted.Should().Be(plainText);
    }

    [Fact]
    public void Encrypt_ShouldProduceDifferentCipherText_ForTheSamePlainText()
    {
        // Arrange
        const string plainText = "MiClaveSecreta1";

        // Act
        var first = _encryptor.Encrypt(plainText);
        var second = _encryptor.Encrypt(plainText);

        // Assert
        first.Should().NotBe(second);
    }
}
