using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Adapters.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Token).HasMaxLength(200).IsRequired();
        builder.Property(t => t.ExpiresAtUtc).IsRequired();
        builder.Property(t => t.CreatedAtUtc).IsRequired();

        builder.HasIndex(t => t.Token).IsUnique();
        builder.HasIndex(t => t.UserId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
