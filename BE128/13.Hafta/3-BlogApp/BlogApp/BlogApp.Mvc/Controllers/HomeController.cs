using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Entities;
using BlogApp.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, DbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            //var userId = GetUserId();

            var blogs = await _dbContext.Set<BlogPostEntity>()
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .Take(10)
                .Select(b => new BlogTableViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Size = b.Content.Length,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();
               

            return View(blogs);
        }

        public async Task<IActionResult> MyBlogs()
        {
            var userId = GetUserId();

            var blogs = await _dbContext.Set<BlogPostEntity>()
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .Take(10)
                .Where(b => b.UserId == userId)
                .Select(b => new BlogTableViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Size = b.Content.Length,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync();


            return View(blogs);
        }

        [HttpGet]
        public IActionResult AddBlog()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBlog([FromForm] NewBlogViewModel newBlogViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userId = GetUserId();

            var blogPost = new BlogPostEntity
            {
                Title = newBlogViewModel.Title,
                Content = newBlogViewModel.Content,
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            _dbContext.Add(blogPost);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail([FromRoute] long id)
        {
            //var userId = GetUserId();

            var blog = await _dbContext.Set<BlogPostEntity>()
                .Include(b => b.User)
                .SingleOrDefaultAsync(b => b.Id == id);

            return View(blog);
        }
        public async Task<IActionResult> DeleteBlog([FromRoute] long id)
        {
            var userId = GetUserId();

            var blog = await _dbContext.Set<BlogPostEntity>()
                .SingleOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (blog is not null)
            {
                _dbContext.Set<BlogPostEntity>().Remove(blog);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("MyBlogs");
        }

        public long GetUserId()
        {
            return long.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? throw new InvalidOperationException());
        }
    }
}
