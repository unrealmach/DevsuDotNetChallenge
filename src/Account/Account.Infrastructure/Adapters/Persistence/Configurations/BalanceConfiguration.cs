using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.Adapters.Persistence.Configurations;

public class BalanceConfiguration : IEntityTypeConfiguration<Balance>
{
    public void Configure(EntityTypeBuilder<Balance> builder)
    {
        builder.ToTable("balance");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.AccountId).IsRequired();
        builder.Property(b => b.Value).IsRequired();

        builder.HasIndex(b => b.AccountId).IsUnique();
        
        builder.HasOne(b => b.Account)
            .WithOne()
            .HasForeignKey<Balance>(b => b.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
