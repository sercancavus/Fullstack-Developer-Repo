namespace App.Data.Contexts;

using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductImageEntity> ProductImages { get; set; }
    public DbSet<ProductCommentEntity> ProductComments { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<CartItemEntity> CartItems { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<ContactFormEntity> ContactForms { get; set; }
    public DbSet<DiscountEntity> ProductDiscounts { get; set; }
    public DbSet<BlogEntity> Blogs { get; set; }
    public DbSet<BlogCommentEntity> BlogComments { get; set; }
    public DbSet<BlogCategoryEntity> BlogCategories { get; set; }
    public DbSet<BlogTagEntity> BlogTags { get; set; }
    public DbSet<RelBlogTagEntity> RelBlogTags { get; set; }
    public DbSet<RelBlogCategoryEntity> RelBlogCategories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DbSeeder.Seed(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProductImageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCommentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CartItemEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ContactFormEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProductDiscountEntityConfiguration());
        modelBuilder.ApplyConfiguration(new BlogEntityConfiguration());
        modelBuilder.ApplyConfiguration(new BlogCommentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new BlogCategoryEntityConfiguration());
        modelBuilder.ApplyConfiguration(new BlogTagEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RelBlogTagEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RelBlogCategoryEntityConfiguration());
    }
}
