using Microsoft.AspNetCore.Mvc;

namespace Admin.Mvc.Controllers
{
    public class UserController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Approve()
        {
            return View();
        }
    }
}