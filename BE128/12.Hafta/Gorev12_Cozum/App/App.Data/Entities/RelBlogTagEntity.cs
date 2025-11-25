using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class RelBlogTagEntity : EntityBase
{
    public int BlogId { get; set; }
    public int TagId { get; set; }

    // Navigation properties
    public BlogEntity Blog { get; set; } = null!;
    public BlogTagEntity Tag { get; set; } = null!;
}

internal class RelBlogTagEntityConfiguration : IEntityTypeConfiguration<RelBlogTagEntity>
{
    public void Configure(EntityTypeBuilder<RelBlogTagEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.BlogId).IsRequired();
        builder.Property(e => e.TagId).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.Blog)
            .WithMany()
            .HasForeignKey(d => d.BlogId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(d => d.Tag)
            .WithMany()
            .HasForeignKey(d => d.TagId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}