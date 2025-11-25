using GameScoreboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoreboard.Data
{
    public static class DataExtensions
    {
        public static IServiceCollection AddGameSbData(this IServiceCollection services, string connectionString)
        {
            // GameScoreboardDbContext isteyen kişi, DbContext ismi ile istesin.

            services.AddDbContext<DbContext, GameScoreboardDbContext>(options =>
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
                    Nickname = "user1",
                    Password = "1234"
                };

                var user2 = new UserEntity
                {
                    Nickname = "user2",
                    Password = "1234"
                };

                var user3 = new UserEntity
                {
                    Nickname = "user3",
                    Password = "1234"
                };

                context.Set<UserEntity>().AddRange(user1, user2, user3);

                await context.SaveChangesAsync();

                var score1 = new HighScoreEntity
                {
                    UserId = user1.Id,
                    Score = 100
                };

                context.Set<HighScoreEntity>().Add(score1);

                await context.SaveChangesAsync();

            }
        }

    }
}
