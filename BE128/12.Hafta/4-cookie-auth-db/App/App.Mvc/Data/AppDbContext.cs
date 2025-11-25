using App.Mvc.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Mvc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<UserEntity> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // UserEntity'nin içindeki Email özelliği unique olarak ayarlanmış oldu.
            modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique();
        }

    }
}
