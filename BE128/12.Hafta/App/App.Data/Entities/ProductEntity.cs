using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class ProductEntity : EntityBase, IHasEnabled
{
    public int SellerId { get; set; }
    public int CategoryId { get; set; }
    public int? DiscountId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    public byte StockAmount { get; set; }
    public bool Enabled { get; set; } = true;

    // Navigation properties
    public UserEntity Seller { get; set; } = null!;
    public CategoryEntity Category { get; set; } = null!;
    public DiscountEntity? Discount { get; set; }

    public ICollection<ProductImageEntity> Images { get; set; } = null!;
    public ICollection<ProductCommentEntity> Comments { get; set; } = null!;
}

internal class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.SellerId).IsRequired();
        builder.Property(e => e.CategoryId).IsRequired();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.Description).HasMaxLength(1000);
        builder.Property(e => e.StockAmount).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.Enabled).IsRequired().HasDefaultValue(true);

        builder.HasOne(d => d.Seller)
            .WithMany()
            .HasForeignKey(d => d.SellerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.Category)
            .WithMany()
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}