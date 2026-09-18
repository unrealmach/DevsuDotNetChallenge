using Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Account.Infrastructure.Adapters.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("client");

        builder.Property(u => u.ClientId).HasMaxLength(160).IsRequired();
        builder.Property(u => u.Status).HasMaxLength(160).IsRequired();

        builder.HasIndex(u => u.ClientId).IsUnique();
    }
}
