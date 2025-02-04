using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Persistence.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    private static readonly Guid UserRoleId = new("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
    private static readonly Guid AdminRoleId = new("2C5E174E-3B0E-446F-86AF-483D56FD7210");

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("AspNetRoles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.NormalizedName)
            .IsRequired()
            .HasMaxLength(100);

        var userRole = Role.Create("USER");
        userRole.GetType().GetProperty("Id")?.SetValue(userRole, UserRoleId);
        userRole.GetType().GetProperty("NormalizedName")?.SetValue(userRole, "USER");

        var adminRole = Role.Create("ADMIN");
        adminRole.GetType().GetProperty("Id")?.SetValue(adminRole, AdminRoleId);
        adminRole.GetType().GetProperty("NormalizedName")?.SetValue(adminRole, "ADMIN");

        builder.HasData(userRole, adminRole);
    }
}