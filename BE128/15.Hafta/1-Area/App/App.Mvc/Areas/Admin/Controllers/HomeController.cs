using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CustomLayout()
        {
            return View();
        }
    }
}
