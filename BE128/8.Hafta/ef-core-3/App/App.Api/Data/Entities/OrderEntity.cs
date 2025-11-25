using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace App.Api.Data.Entities
{
    public class OrderEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }


        // navigation property

        //[ForeignKey(nameof(CustomerId))]
        [JsonIgnore] // bu olmadığında order'lar ve customer'lar arasında sonsuz döngü olur.
        public CustomerEntity Customer { get; set; } = null!;

    }

    public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            // Siparişe tarih verilmediğinde içinde bulunduğumuz tarihi atayacak.
            builder.Property(x => x.OrderDate).HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId);

            //builder.Property("OrderNumber");
            builder.Property(nameof(OrderEntity.OrderNumber)).IsRequired();
        }
    }

}
