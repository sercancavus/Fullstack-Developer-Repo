using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class BlogTagEntity : EntityBase
{
    public string Name { get; set; } = null!;
}

internal class BlogTagEntityConfiguration : IEntityTypeConfiguration<BlogTagEntity>
{
    public void Configure(EntityTypeBuilder<BlogTagEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
        builder.Property(e => e.CreatedAt).IsRequired();
    }
}
