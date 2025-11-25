using Microsoft.EntityFrameworkCore;
using App.Data.Entities;

namespace App.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<ProductComment> ProductComments => Set<ProductComment>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Global defaults
            modelBuilder.Entity<User>().Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Role>().Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Category>().Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Product>().Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<ProductImage>().Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<ProductComment>().Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Order>().Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<OrderItem>().Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<CartItem>().Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Relations / constraints examples (basic)
            modelBuilder.Entity<User>()
                .HasOne(r => r.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany()
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductComment>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProductId);

            modelBuilder.Entity<ProductComment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductImage>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed static data with fixed dates for repeatability
            var seedDate = new DateTime(2025,1,1,0,0,0,DateTimeKind.Utc);
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "admin", CreatedAt = seedDate },
                new Role { Id = 2, Name = "seller", CreatedAt = seedDate },
                new Role { Id = 3, Name = "buyer", CreatedAt = seedDate }
            );
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "admin@example.com", FirstName = "Admin", LastName = "User", Password = "admin123", RoleId = 1, Enabled = true, CreatedAt = seedDate }
            );
            var categories = new List<Category>();
            for (int i = 1; i <= 10; i++)
            {
                categories.Add(new Category { Id = i, Name = $"Category {i}", Color = "FFF", IconCssClass = "icon-cat", CreatedAt = seedDate });
            }
            modelBuilder.Entity<Category>().HasData(categories.ToArray());
        }
    }
}