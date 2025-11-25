using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class SectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            List<string> userNames = new()
            {
                "Ali",
                "Veli",
                "Mahmut",
                "Cemil",
                "Ayşe"
            };

            ViewData["users"] = userNames;

            return View();
        }
    }
}
