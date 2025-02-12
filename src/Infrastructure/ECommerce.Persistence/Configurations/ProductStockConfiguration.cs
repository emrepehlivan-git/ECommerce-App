using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistence.Configurations;

internal sealed class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProductStock> builder)
    {
        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Quantity)
            .IsRequired();

        builder.HasOne(ps => ps.Product)
            .WithOne()
            .HasForeignKey<ProductStock>(ps => ps.ProductId);

        builder.HasIndex(ps => ps.ProductId);
    }
}
