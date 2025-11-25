using App.Mvc.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Mvc.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
            
        }

        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<UserEntity> users = new();

            for (int i = 0; i < 5; i++)
            {
                users.Add(new UserEntity { Id = i + 1, Name = $"User{i + 1}" });
            }

            modelBuilder.Entity<UserEntity>().HasData(users);

            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        }

    }
}
