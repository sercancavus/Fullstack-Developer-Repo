using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class BlogCategoryEntity : EntityBase
{
    public string Name { get; set; } = null!;
}

internal class BlogCategoryEntityConfiguration : IEntityTypeConfiguration<BlogCategoryEntity>
{
    public void Configure(EntityTypeBuilder<BlogCategoryEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
        builder.Property(e => e.CreatedAt).IsRequired();
    }
}
