using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class RelBlogCategoryEntity : EntityBase
{
    public int BlogId { get; set; }
    public int CategoryId { get; set; }

    // Navigation properties
    public BlogEntity Blog { get; set; } = null!;
    public BlogCategoryEntity Category { get; set; } = null!;
}

internal class RelBlogCategoryEntityConfiguration : IEntityTypeConfiguration<RelBlogCategoryEntity>
{
    public void Configure(EntityTypeBuilder<RelBlogCategoryEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.BlogId).IsRequired();
        builder.Property(e => e.CategoryId).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.Blog)
            .WithMany()
            .HasForeignKey(d => d.BlogId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.Category)
            .WithMany()
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
