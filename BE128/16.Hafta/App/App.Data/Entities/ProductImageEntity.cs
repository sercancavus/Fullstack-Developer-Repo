using App.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class ProductImageEntity : EntityBase
{
    public int ProductId { get; set; }
    public string Url { get; set; } = null!;

    // Navigation properties
    public ProductEntity Product { get; set; } = null!;
}

internal class ProductImageEntityConfiguration : IEntityTypeConfiguration<ProductImageEntity>
{
    public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.Url).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        new ProductImageEntitySeed().SeedData(builder);
    }
}

internal class ProductImageEntitySeed : IEntityTypeSeed<ProductImageEntity>
{
    public void SeedData(EntityTypeBuilder<ProductImageEntity> builder)
    {
        builder.HasData(
            new List<ProductImageEntity>
            {
                new() { Id = 1, ProductId = 1, Url = "/theme/img/product/discount/pd-3.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 2, ProductId = 2, Url = "/theme/img/product/discount/pd-4.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 3, ProductId = 3, Url = "/theme/img/product/discount/pd-5.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 4, ProductId = 4, Url = "/theme/img/product/product-1.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 5, ProductId = 5, Url = "/theme/img/product/product-2.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 6, ProductId = 6, Url = "/theme/img/product/product-3.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 7, ProductId = 7, Url = "/theme/img/product/product-8.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 8, ProductId = 8, Url = "/theme/img/product/product-4.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 9, ProductId = 9, Url = "/theme/img/product/product-7.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 10, ProductId = 10, Url = "/theme/img/product/product-9.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 11, ProductId = 11, Url = "/theme/img/product/product-11.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 12, ProductId = 12, Url = "/theme/img/product/product-12.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 13, ProductId = 13, Url = "/theme/img/latest-product/lp-1.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 14, ProductId = 14, Url = "/theme/img/product/details/product-details-2.jpg", CreatedAt = DateTime.UtcNow },
                new() { Id = 15, ProductId = 15, Url = "/theme/img/product/product-10.jpg", CreatedAt = DateTime.UtcNow }
            }
        );
    }
}