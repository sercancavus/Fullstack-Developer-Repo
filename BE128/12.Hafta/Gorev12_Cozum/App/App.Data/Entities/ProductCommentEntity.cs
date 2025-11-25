using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class ProductCommentEntity : EntityBase
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; } = null!;
    public byte StarCount { get; set; }
    public bool IsConfirmed { get; set; } = false;

    // Navigation properties
    public ProductEntity Product { get; set; } = null!;
    public UserEntity User { get; set; } = null!;
}

internal class ProductCommentEntityConfiguration : IEntityTypeConfiguration<ProductCommentEntity>
{
    public void Configure(EntityTypeBuilder<ProductCommentEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.Text).IsRequired().HasMaxLength(500);
        builder.Property(e => e.StarCount).IsRequired().HasDefaultValue(3); // 3 stars by default
        builder.Property(e => e.IsConfirmed).IsRequired().HasDefaultValue(false); // Not confirmed by default
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.Product)
            .WithMany(e => e.Comments)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}