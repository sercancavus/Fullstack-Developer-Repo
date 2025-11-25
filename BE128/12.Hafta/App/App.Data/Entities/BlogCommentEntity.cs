using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class BlogCommentEntity : EntityBase
{
    public int BlogId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Comment { get; set; } = null!;
    public bool IsApproved { get; set; }

    // Navigation properties
    public BlogEntity Blog { get; set; } = null!;
}

internal class BlogCommentEntityConfiguration : IEntityTypeConfiguration<BlogCommentEntity>
{
    public void Configure(EntityTypeBuilder<BlogCommentEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.BlogId).IsRequired();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Comment).IsRequired();
        builder.Property(e => e.IsApproved).IsRequired().HasDefaultValue(false);
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(e => e.Blog)
            .WithMany(e => e.Comments)
            .HasForeignKey(e => e.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
