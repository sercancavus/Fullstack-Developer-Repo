using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class BlogEntity : EntityBase, IHasEnabled
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public int UserId { get; set; }
    public bool Enabled { get; set; } = true;

    // Navigation properties
    public UserEntity User { get; set; } = null!;

    public ICollection<BlogCommentEntity> Comments { get; set; } = null!;
}

internal class BlogEntityConfiguration : IEntityTypeConfiguration<BlogEntity>
{
    public void Configure(EntityTypeBuilder<BlogEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Content).IsRequired();
        builder.Property(e => e.ImageUrl).HasMaxLength(256);
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.Enabled).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
