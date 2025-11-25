using App.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class RoleEntity : EntityBase
{
    public string Name { get; set; } = null!;
}

internal class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(10);
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasIndex(e => e.Name).IsUnique();

        new RoleEntitySeed().SeedData(builder);
    }
}

internal class RoleEntitySeed : IEntityTypeSeed<RoleEntity>
{
    public void SeedData(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasData(
            new RoleEntity() { Id = 1, Name = "admin", CreatedAt = DateTime.UtcNow },
            new RoleEntity() { Id = 2, Name = "seller", CreatedAt = DateTime.UtcNow },
            new RoleEntity() { Id = 3, Name = "buyer", CreatedAt = DateTime.UtcNow }
        );
    }
}