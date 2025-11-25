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
            if (!HttpContext.Session.Keys.Contains("view-count"))
            {
                HttpContext.Session.SetInt32("view-count", 0);
            }

            int count = HttpContext.Session.GetInt32("view-count") ?? throw new InvalidOperationException();

            count++;

            HttpContext.Session.SetInt32("view-count", count);

            return View();
        }

        public IActionResult Privacy()
        {
            HttpContext.Session.Remove("view-count");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
