using Microsoft.AspNetCore.Mvc;

namespace Admin.Mvc.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Delete()
        {
            return View();
        }
    }
}