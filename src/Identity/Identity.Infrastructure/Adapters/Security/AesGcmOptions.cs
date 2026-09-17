using System.ComponentModel.DataAnnotations;

namespace Identity.Infrastructure.Adapters.Security;

public sealed class AesGcmOptions
{
    public const string SectionName = "CredentialEncryption";

    [Required]
    public string Key { get; init; } = string.Empty;
}
