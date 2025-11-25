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
        builder.Property(e => e.Url).IsRequired().HasMaxLength(250);
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}