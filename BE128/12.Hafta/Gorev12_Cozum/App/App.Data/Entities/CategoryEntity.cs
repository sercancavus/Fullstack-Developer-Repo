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
        builder.Property(e => e.Color).IsRequired().HasMaxLength(6);
        builder.Property(e => e.IconCssClass).IsRequired().HasMaxLength(50);
        builder.Property(e => e.CreatedAt).IsRequired();
    }
}