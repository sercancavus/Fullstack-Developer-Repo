using Microsoft.AspNetCore.Mvc;

namespace Admin.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}