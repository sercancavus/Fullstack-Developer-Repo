using GameScoreboard.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoreboard.Data
{
    internal class GameScoreboardDbContext : DbContext
    {
        public GameScoreboardDbContext(DbContextOptions<GameScoreboardDbContext> options) : base (options)
        {
            
        }

        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<HighScoreEntity> HighScores { get; set; } = null!;

    }
}
