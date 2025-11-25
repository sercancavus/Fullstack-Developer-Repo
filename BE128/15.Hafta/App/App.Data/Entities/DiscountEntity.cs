using App.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities
{
    public class DiscountEntity : EntityBase, IHasEnabled
    {
        public byte DiscountRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Enabled { get; set; } = true;
    }

    internal class DiscountEntityConfiguration : IEntityTypeConfiguration<DiscountEntity>
    {
        public void Configure(EntityTypeBuilder<DiscountEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Enabled).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).IsRequired();

            new DiscountEntitySeed().SeedData(builder);
        }
    }

    internal class DiscountEntitySeed : IEntityTypeSeed<DiscountEntity>
    {
        public void SeedData(EntityTypeBuilder<DiscountEntity> builder)
        {
            builder.HasData(
                new List<DiscountEntity>
                {
                    new() { Id = 1, DiscountRate = 10, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(6), Enabled = true, CreatedAt = DateTime.UtcNow },
                    new() { Id = 2, DiscountRate = 20, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(6), Enabled = true, CreatedAt = DateTime.UtcNow },
                    new() { Id = 3, DiscountRate = 30, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(6), Enabled = true, CreatedAt = DateTime.UtcNow }
                }
            );
        }
    }
}