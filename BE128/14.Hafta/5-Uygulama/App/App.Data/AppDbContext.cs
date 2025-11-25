using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
            
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<UserEntity> users = new();
            List<ProductEntity> products = new();

            for (int i = 0; i < 5; i++)
            {
                users.Add(new UserEntity { Id = i + 1, Name = $"User{i + 1}" });
            }

            for (int i = 0; i < 5; i++)
            {
                products.Add(new ProductEntity { Id = i + 1, Name = $"Product{i + 1}" });
            }

            modelBuilder.Entity<UserEntity>().HasData(users);
            modelBuilder.Entity<ProductEntity>().HasData(products);

            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
        }

    }
}
