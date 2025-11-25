using App.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class UserEntity : EntityBase, IHasEnabled
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int RoleId { get; set; }
    public bool Enabled { get; set; } = true;
    public bool HasSellerRequest { get; set; } = false;

    // Navigation properties
    public RoleEntity Role { get; set; } = null!;
}

internal class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(256);
        builder.HasIndex(e => e.Email).IsUnique();
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Password).IsRequired();
        builder.Property(e => e.RoleId).IsRequired();
        builder.Property(e => e.Enabled).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.Role)
            .WithMany()
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        new UserEntitySeed().SeedData(builder);
    }
}

internal class UserEntitySeed : IEntityTypeSeed<UserEntity>
{
    public void SeedData(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasData(
            new UserEntity() { Id = 1, FirstName = "admin", LastName = "admin", Email = "admin@siliconmade.com", Enabled = true, RoleId = 1, Password = "1234", CreatedAt = DateTime.UtcNow },
            new UserEntity() { Id = 2, FirstName = "seller", LastName = "seller", Email = "seller@siliconmade.com", Enabled = true, RoleId = 2, Password = "1234", CreatedAt = DateTime.UtcNow },
            new UserEntity() { Id = 3, FirstName = "buyer", LastName = "buyer", Email = "buyer@siliconmade.com", Enabled = true, RoleId = 3, Password = "1234", CreatedAt = DateTime.UtcNow }
        );
    }
}