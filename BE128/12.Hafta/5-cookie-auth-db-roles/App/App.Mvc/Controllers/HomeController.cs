using System.Diagnostics;
using App.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

    }
}
