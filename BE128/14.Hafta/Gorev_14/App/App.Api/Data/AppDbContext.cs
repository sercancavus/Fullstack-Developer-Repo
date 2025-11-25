using App.Api.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data
{
    public class AppDbContext : IdentityDbContext<App.Api.Data.Entities.User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>().HasData(
                new Microsoft.AspNetCore.Identity.IdentityRole { Id = "1", Name = "Buyer", NormalizedName = "BUYER" },
                new Microsoft.AspNetCore.Identity.IdentityRole { Id = "2", Name = "Seller", NormalizedName = "SELLER" },
                new Microsoft.AspNetCore.Identity.IdentityRole { Id = "3", Name = "Admin", NormalizedName = "ADMIN" }
            );
        }
    }
}