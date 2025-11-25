using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class TodoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
