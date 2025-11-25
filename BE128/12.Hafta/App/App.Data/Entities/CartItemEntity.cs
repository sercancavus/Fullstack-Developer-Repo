using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class CartItemEntity : EntityBase
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public byte Quantity { get; set; }

    // Navigation properties
    public UserEntity User { get; set; } = null!;
    public ProductEntity Product { get; set; } = null!;
}

internal class CartItemEntityConfiguration : IEntityTypeConfiguration<CartItemEntity>
{
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.Quantity).IsRequired().HasDefaultValue(1); // 1 item by default
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.Product)
            .WithMany()
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
