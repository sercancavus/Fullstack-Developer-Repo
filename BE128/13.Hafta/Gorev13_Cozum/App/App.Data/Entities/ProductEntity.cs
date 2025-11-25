using App.Data.Infrastructure;
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

        builder.HasOne(d => d.Discount)
            .WithMany()
            .HasForeignKey(d => d.DiscountId)
            .OnDelete(DeleteBehavior.NoAction);

        new ProductEntitySeed().SeedData(builder);
    }
}

internal class ProductEntitySeed : IEntityTypeSeed<ProductEntity>
{
    private static readonly Random _rnd = Random.Shared;

    public void SeedData(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasData(
                new List<ProductEntity>{
                    new() {
                        Id = 1,
                        Name = "Mixed Fruit Juice",
                        CategoryId = 9,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Mixed Fruit Juice",
                        StockAmount = 100,
                        CreatedAt = DateTime.UtcNow,
                        DiscountId = 2
                    },
                    new() {
                        Id = 2,
                        Name = "Mango",
                        CategoryId = 3,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Mango",
                        StockAmount = 50,
                        CreatedAt = DateTime.UtcNow,
                        DiscountId = 2
                    },
                    new() {
                        Id = 3,
                        Name = "Hamburger",
                        CategoryId = 7,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Hamburger",
                        StockAmount = 20,
                        CreatedAt = DateTime.UtcNow,
                        DiscountId = 2
                    },
                    new() {
                        Id = 4,
                        Name = "Red Meat",
                        CategoryId = 1,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Meat",
                        StockAmount = 50,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new() {
                        Id = 5,
                        Name = "Banana",
                        CategoryId = 3,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Banana",
                        StockAmount = 75,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new() {
                        Id = 6,
                        Name = "Fig",
                        CategoryId = 3,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Fig",
                        StockAmount = 100,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new() {
                        Id = 7,
                        Name = "Apple",
                        CategoryId = 3,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Apple",
                        StockAmount = 80,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new() {
                        Id = 8,
                        Name = "Grapes",
                        CategoryId = 3,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Grapes",
                        StockAmount = 100,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new() {
                        Id = 9,
                        Name = "Watermelon",
                        CategoryId = 3,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Watermelon",
                        StockAmount = 20,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new () {
                        Id = 10,
                        Name = "Raisins",
                        CategoryId = 4,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Raisins",
                        StockAmount = 100,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new() {
                        Id = 11,
                        Name = "Orange Juice",
                        CategoryId = 9,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Orange Juice",
                        StockAmount = 100,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new() {
                        Id = 12,
                        Name = "Mixed Fruits",
                        CategoryId = 3,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Mixed Fruits",
                        StockAmount = 100,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new() {
                        Id = 13,
                        Name = "Spinach",
                        CategoryId = 2,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Spinach",
                        StockAmount = 100,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new() {
                        Id = 14,
                        Name = "Bell Pepper",
                        CategoryId = 2,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Bell Pepper",
                        StockAmount = 100,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new() {
                        Id = 15,
                        Name = "Fried Chicken",
                        CategoryId = 7,
                        SellerId = 2,
                        Price = _rnd.Next(10, 540),
                        Description = "Fried Chicken",
                        StockAmount = 20,
                        CreatedAt = DateTime.UtcNow,
                    }
                }
            );
    }
}