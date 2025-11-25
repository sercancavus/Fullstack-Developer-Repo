using BlogApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Data
{
    internal class BlogDbContext :DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<BlogPostEntity> BlogPosts { get; set; } = null!;
    }
}
