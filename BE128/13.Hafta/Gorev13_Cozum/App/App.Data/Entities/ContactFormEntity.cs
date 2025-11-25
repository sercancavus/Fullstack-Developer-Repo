using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class ContactFormEntity : EntityBase
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime? SeenAt { get; set; }
}

internal class ContactFormEntityConfiguration : IEntityTypeConfiguration<ContactFormEntity>
{
    public void Configure(EntityTypeBuilder<ContactFormEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(256);
        builder.Property(e => e.Message).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.CreatedAt).IsRequired();
    }
}