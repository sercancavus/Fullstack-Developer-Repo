using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
