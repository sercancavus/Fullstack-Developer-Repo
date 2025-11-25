using App.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class CategoryEntity : EntityBase
{
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string IconCssClass { get; set; } = null!;
}

internal class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Color).IsRequired();
        builder.Property(e => e.IconCssClass).IsRequired().HasMaxLength(50);
        builder.Property(e => e.CreatedAt).IsRequired();

        new CategoryEntitySeed().SeedData(builder);
    }
}

internal class CategoryEntitySeed : IEntityTypeSeed<CategoryEntity>
{
    public void SeedData(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasData(
            new List<CategoryEntity>{
                new() { Id = 1, Name = "Fresh Meat", Color = "Blue", IconCssClass = string.Empty, CreatedAt = DateTime.UtcNow },
                new() { Id = 2, Name = "Vegetables", Color = "Red", IconCssClass = string.Empty, CreatedAt = DateTime.UtcNow },
                new() { Id = 3, Name = "Fresh Fruits", Color = "Green", IconCssClass = string.Empty, CreatedAt = DateTime.UtcNow },
                new() { Id = 4, Name = "Dried Fruits & Nuts", Color = "Brown", IconCssClass = string.Empty, CreatedAt = DateTime.UtcNow },
                new() { Id = 5, Name = "Ocean Foods", Color = "Purple", IconCssClass = string.Empty, CreatedAt = DateTime.Now },
                new() { Id = 6, Name = "Butter & Eggs", Color = "Yellow", IconCssClass = string.Empty, CreatedAt = DateTime.UtcNow },
                new() { Id = 7, Name = "Fastfood", Color = "Pink", IconCssClass = string.Empty, CreatedAt = DateTime.UtcNow },
                new() { Id = 8, Name = "Oatmeal", Color = "Grey", IconCssClass = string.Empty, CreatedAt = DateTime.UtcNow },
                new() { Id = 9, Name = "Juices", Color = "Orange", IconCssClass = string.Empty, CreatedAt = DateTime.UtcNow }
            }
        );
    }
}