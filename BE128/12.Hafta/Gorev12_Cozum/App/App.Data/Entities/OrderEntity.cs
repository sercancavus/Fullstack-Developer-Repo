using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class OrderEntity : EntityBase
{
    public int UserId { get; set; }
    public string OrderCode { get; set; } = null!;
    public string Address { get; set; } = null!;

    // Navigation properties
    public UserEntity User { get; set; } = null!;
}

internal class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.OrderCode).IsRequired().HasMaxLength(250);
        builder.Property(e => e.Address).IsRequired().HasMaxLength(250);
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}