using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.FullName, fullName =>
        {
            fullName.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            fullName.Property(x => x.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
    }
}