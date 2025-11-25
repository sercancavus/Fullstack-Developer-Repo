using App.Mvc.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Mvc.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (await db.Users.AnyAsync())// hiç eleman var mı?
            {
                return;
            }

            var users = new[]
            {
                new UserEntity
                {
                    Name = "Mahmut",
                    Email = "mahmut@mahmut.com",
                    Password = "1234"
                },
                new UserEntity
                {
                    Name = "Cemil",
                    Email = "cemil@cemil.com",
                    Password = "1234"
                }
            };

            // AddRange : DbSet'lere toplu halde veri eklemek için kullanılabilir.

            db.Users.AddRange(users);

            await db.SaveChangesAsync();
        }
    }
}
