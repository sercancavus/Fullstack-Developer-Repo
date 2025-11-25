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

    internal class ProductDiscountEntityConfiguration : IEntityTypeConfiguration<DiscountEntity>
    {
        public void Configure(EntityTypeBuilder<DiscountEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Enabled).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.CreatedAt).IsRequired();
        }
    }
}
