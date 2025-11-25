using System.Diagnostics;
using App.Mvc.Models;
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

        public IActionResult Index()
        {
            // Kullanýcý login olmuþ mu kontrolü
            // Kullanývý login olmamýþsa
            if (!Request.Cookies.Keys.Contains("user"))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

    }
}
