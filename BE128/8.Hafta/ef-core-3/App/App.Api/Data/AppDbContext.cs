using App.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // veritabanı için program.cs'de build metodu üzerinde yapılan ayarların (bilgilerin) AppDbContext'e aktarılması (inject edilmesi) sağlanır.
        }

        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // OrderEntity için yazılan konfigurasyonları uygula
            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
        }

    }
}
