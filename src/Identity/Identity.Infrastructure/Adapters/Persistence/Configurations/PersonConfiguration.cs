using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Adapters.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("person");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).HasMaxLength(160).IsRequired();
        builder.Property(u => u.Gender).HasMaxLength(10).IsRequired();
        builder.Property(u => u.Age).HasMaxLength(5).IsRequired();
        builder.Property(u => u.Identification).HasMaxLength(20).IsRequired();
        builder.Property(u => u.Address).IsRequired();
        builder.Property(u => u.Phone).IsRequired();

    }
}