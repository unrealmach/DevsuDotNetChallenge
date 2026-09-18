using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.Adapters.Persistence.Configurations;

public class HistoryConfiguration : IEntityTypeConfiguration<History>
{
    public void Configure(EntityTypeBuilder<History> builder)
    {
        builder.ToTable("history");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.ClientId).IsRequired();
        builder.Property(h => h.AccountId).IsRequired();
        builder.Property(h => h.Date).IsRequired();
        builder.Property(h => h.Value).IsRequired();
        builder.Property(h => h.Type).HasMaxLength(20).IsRequired();
        builder.Property(h => h.AvailableBalance).IsRequired();

        builder.HasIndex(h => h.ClientId);
        builder.HasIndex(h => h.AccountId);
        
        builder.HasOne(h => h.Client)
            .WithMany()
            .HasForeignKey(h => h.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.Account)
            .WithMany()
            .HasForeignKey(h => h.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
