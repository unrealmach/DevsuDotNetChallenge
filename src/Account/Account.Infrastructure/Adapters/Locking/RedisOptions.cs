using System.ComponentModel.DataAnnotations;

namespace Account.Infrastructure.Adapters.Locking;

public sealed class RedisOptions
{
    public const string SectionName = "Redis";

    [Required]
    public string ConnectionString { get; init; } = string.Empty;
}
