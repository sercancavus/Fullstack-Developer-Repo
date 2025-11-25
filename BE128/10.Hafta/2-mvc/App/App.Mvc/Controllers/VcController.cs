using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class VcController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
