using App.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Data.Entities;

public class BlogEntity : EntityBase, IHasEnabled
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public int UserId { get; set; }
    public bool Enabled { get; set; } = true;

    // Navigation properties
    public UserEntity User { get; set; } = null!;

    public ICollection<BlogCommentEntity> Comments { get; set; } = null!;
}

internal class BlogEntityConfiguration : IEntityTypeConfiguration<BlogEntity>
{
    public void Configure(EntityTypeBuilder<BlogEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Content).IsRequired();
        builder.Property(e => e.ImageUrl).HasMaxLength(256);
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.Enabled).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        new BlogEntitySeed().SeedData(builder);
    }
}

internal class BlogEntitySeed : IEntityTypeSeed<BlogEntity>
{
    public void SeedData(EntityTypeBuilder<BlogEntity> builder)
    {
        builder.HasData(
            new List<BlogEntity>
            {
                new()
                {
                    Id = 1,
                    Title = "Visit the clean farm in the US",
                    Content = "Visiting a clean farm in the US is an enlightening experience. The farm, with its well-maintained fields and healthy livestock, showcases the best of American agriculture. The cleanliness of the farm is a testament to the hard work and dedication of the farmers. They follow strict hygiene and cleanliness standards to ensure the health and safety of their animals and crops. The farm also uses sustainable farming practices to protect the environment. A visit to such a farm is not only educational but also inspiring, as it shows the importance of cleanliness and sustainability in farming.",
                    ImageUrl = "/theme/img/blog/blog-3.jpg",
                    UserId = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = 2,
                    Title = "6 ways to prepare breakfast for 30",
                    Content = "Preparing breakfast for a large group requires careful planning and organization. Start by choosing recipes that can be made in large quantities, such as scrambled eggs, pancakes, or a breakfast casserole. Consider making items that can be prepared ahead of time, like muffins or fruit salad. Set up a serving station with all the necessary utensils, plates, and cups. Cook as much as you can in advance to minimize stress on the day of the event. Remember to cater to different dietary needs and preferences. With these tips, you'll be able to host a successful breakfast for 30 people.",
                    ImageUrl = "/theme/img/blog/blog-2.jpg",
                    UserId = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = 3,
                    Title = "Cooking tips make cooking simple",
                    Content = "Cooking can be a daunting task for many, especially when it comes to preparing meals for a family or a large group. However, with the right tips and techniques, it can be made simple and enjoyable. Start by planning your meals in advance and doing a weekly grocery shop. This not only saves time but also ensures you have all the ingredients you need. Use fresh ingredients wherever possible as they provide the best flavor. Don't be afraid to experiment with different herbs and spices to add a unique twist to your dishes. Remember, the key to simple cooking is preparation and using the right tools. Invest in a good set of knives, pots, and pans. Lastly, don't forget to clean as you go. This will keep your kitchen tidy and make the cooking process much more manageable.",
                    ImageUrl = "/theme/img/blog/blog-1.jpg",
                    UserId = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = 4,
                    Title = "The benefits of eating organic food",
                    Content = "Organic food has become increasingly popular in recent years, with many people choosing to buy organic produce over conventionally grown food. There are several benefits to eating organic food, including better taste, higher nutritional value, and fewer pesticides. Organic farming practices are also better for the environment, as they reduce pollution and conserve water and soil. Additionally, organic food is often fresher and free from harmful additives and preservatives. While organic food can be more expensive, many people believe the health benefits are worth the extra cost. Overall, eating organic food is a great way to support your health and the environment.",
                    ImageUrl = "/theme/img/blog/blog-4.jpg",
                    UserId = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = 5,
                    Title = "How to (and not to) include fruits in your diet",
                    Content = "Fruits are an essential part of a healthy diet, providing essential vitamins, minerals, and fiber. However, it's essential to consume them in moderation and avoid overeating. While fruits are nutritious, they also contain natural sugars that can contribute to weight gain if consumed in excess. To include fruits in your diet, aim to eat a variety of colors and types to ensure you're getting a wide range of nutrients. Fresh, frozen, and dried fruits are all healthy options, but be mindful of added sugars in canned or packaged fruits. Avoid fruit juices and smoothies, as they can be high in sugar and calories. Instead, opt for whole fruits, which provide more fiber and nutrients. By following these tips, you can enjoy the health benefits of fruits without overdoing it.",
                    ImageUrl = "/theme/img/blog/blog-5.jpg",
                    UserId = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = 6,
                    Title = "The moment You Need To Remove Garlic From The Menu",
                    Content = "Garlic is a popular ingredient in many dishes, known for its strong flavor and health benefits. However, there are times when you may need to remove garlic from the menu. Some people are allergic to garlic and can experience symptoms like hives, itching, or difficulty breathing after consuming it. If you're cooking for someone with a garlic allergy, it's essential to avoid using garlic in your dishes. Additionally, some people may have a sensitivity to garlic that causes digestive issues like bloating or gas. In these cases, it's best to limit or avoid garlic in your cooking. While garlic is a versatile and flavorful ingredient, it's essential to be mindful of people's dietary needs and preferences when preparing meals.",
                    ImageUrl = "/theme/img/blog/blog-6.jpg",
                    UserId = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = 7,
                    Title = "Cost anaylsis of organic food",
                    Content = "Organic food has become increasingly popular in recent years, with many people choosing to buy organic produce over conventionally grown food. While organic food is often more expensive than non-organic options, there are several factors that contribute to the higher cost. Organic farming practices are more labor-intensive and require more time and effort than conventional farming methods. Additionally, organic farmers often pay higher prices for organic seeds, fertilizers, and pest control methods. The certification process for organic food is also costly, as farmers must meet strict standards to be certified organic. While the cost of organic food can be a barrier for some consumers, many people believe the health and environmental benefits are worth the extra expense.",
                    ImageUrl = "/theme/img/blog/blog-1.jpg",
                    UserId = 1,
                    CreatedAt = DateTime.UtcNow
                }
            }
        );
    }
}