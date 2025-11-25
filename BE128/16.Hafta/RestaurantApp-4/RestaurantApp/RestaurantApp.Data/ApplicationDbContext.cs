using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProducts> SalesProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<SaleProducts>(entity =>
            {
                entity.Property(e => e.ProductPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ProductTotalPrice).HasColumnType("decimal(18,2)");

            });

            modelBuilder.Entity<SaleProducts>()
                .HasOne(x => x.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<SaleProducts>()
                .HasKey(sp => new { sp.SaleId, sp.ProductId });

            modelBuilder.Entity<SaleProducts>()
                .HasOne(s => s.Sale)
                .WithMany(s => s.SaleProducts)
                .HasForeignKey(s => s.SaleId);

            modelBuilder.Entity<SaleProducts>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }

    }
}
