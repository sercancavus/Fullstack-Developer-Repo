using BlogApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Data
{
    public static class DataExtensions
    {
        public static IServiceCollection AddBlogAppData(this IServiceCollection services, string connectionString)
        {
            // GameScoreboardDbContext isteyen kişi, DbContext ismi ile istesin.

            services.AddDbContext<DbContext, BlogDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }

        public static async Task EnsureCreatedAndSeedAsync(this DbContext context)
        {
            if (await context.Database.EnsureCreatedAsync())
            {
                // seed

                var user1 = new UserEntity
                {
                    Email = "user1@gmail.com",
                    Password = "1234"
                };

                var user2 = new UserEntity
                {
                    Email = "user2@gmail.com",
                    Password = "1234"
                };

                var user3 = new UserEntity
                {
                    Email = "user3@gmail.com",
                    Password = "1234"
                };

                context.Set<UserEntity>().AddRange(user1, user2, user3);

                await context.SaveChangesAsync();

                var post1 = new BlogPostEntity
                {
                    UserId = user1.Id,
                    Title = "First Post",
                    Content = "<h1>This is the first post</h1>",
                    CreatedAt = DateTime.Now
                };

                context.Set<BlogPostEntity>().Add(post1);

                await context.SaveChangesAsync();

            }
        }
    }
}
