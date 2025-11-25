using App.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data
{
    public class AppDbContext : DbContext
    {
        // AppDbContext -> Veritabanına kaşılık gelen class

        // projeye dahil edilmesi gereken 2 paket;
        // 1) Microsoft.EntityFrameworkCore


        public DbSet<UserModel> Users { get; set; } // Veritabanındaki Users tablolasunu ifade eder


        // Veritabanına bağlanmak için gerekli bilgiler lazım!
        // Bunun için "Connection string" denilen bir ifade kullanılır.

        //private string _connectionString = "Server=.;Database=be128_ef_1;Trusted_Connection=True;TrustServerCertificate=Yes";


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=be128_ef_1;Trusted_Connection=True;TrustServerCertificate=Yes");
        }

    }
}
