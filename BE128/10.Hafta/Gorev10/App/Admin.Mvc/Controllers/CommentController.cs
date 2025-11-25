using Microsoft.AspNetCore.Mvc;

namespace Admin.Mvc.Controllers
{
    public class CommentController : Controller
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