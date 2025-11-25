using System.Diagnostics;
using App.Mvc.Data;
using App.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Moderator")]
        public IActionResult Moderator()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Moderator")] // Admin veya moderator. "," -> or/veya gibi
        public IActionResult AdminOrModerator()
        {
            return View();
        }

        // Hem admin rolü hem moderator rolü olanlar girebilir.
        // þu anda veritabaný yapýsý buna uygun deðil! 
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Moderator")]
        public IActionResult AdminAndModerator()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnapprovedUsers()
        {
            // DB'den sorgulanan UserEntity'ler UserViewModel türüne dönüþtürülür.(Select metodu ile)

            // Select metodu modelleri birbirne dönüþtürmek için kullanýlýr.

            var users = await _dbContext.Users
                .Where(x => !x.IsApproved)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email
                })
                .ToListAsync();

            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == id && !x.IsApproved);

            if (user is null)
            {
                return NotFound();
            }

            user.IsApproved = true;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("UnapprovedUsers");
        }

    }
}
