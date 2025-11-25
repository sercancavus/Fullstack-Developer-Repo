using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;
public class OrderItemEntity : EntityBase
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public byte Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Navigation properties
    public OrderEntity Order { get; set; } = null!;
    public ProductEntity Product { get; set; } = null!;
}

internal class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.OrderId).IsRequired();
        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.Quantity).IsRequired().HasDefaultValue(1); // 1 item by default
        builder.Property(e => e.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.Order)
            .WithMany()
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.Product)
            .WithMany()
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}