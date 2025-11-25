using App.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; } // Veritabanındaki Users tablolasunu ifade eder
        public DbSet<TodoEntity> Todos { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=be128_ef_2;Trusted_Connection=True;TrustServerCertificate=Yes");
        //}

    }
}
