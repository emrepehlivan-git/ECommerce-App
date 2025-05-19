using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Persistence.Configurations;

public sealed class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(EntityTypeBuilder<ProductStock> builder)
    {
        builder.ToTable("product_stocks");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.Quantity)
            .IsRequired();

        builder.Property(ps => ps.ProductId)
            .IsRequired();

        builder.HasOne(ps => ps.Product)
            .WithOne(p => p.Stock)
            .HasForeignKey<ProductStock>(ps => ps.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ps => ps.ProductId)
            .IsUnique();
    }
}
