using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}