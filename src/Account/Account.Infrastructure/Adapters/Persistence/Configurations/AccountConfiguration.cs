using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AccountEntity = Account.Domain.Entities.Account;

namespace Account.Infrastructure.Adapters.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.ToTable("account");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.ClientId).IsRequired();
        builder.Property(a => a.Type).HasMaxLength(20).IsRequired();
        builder.Property(a => a.Status).HasMaxLength(20).IsRequired();

        builder.HasIndex(a => a.ClientId);

        builder.HasOne(a => a.Client)
            .WithMany()
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
