using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    public class BlogController : BaseController
    {
        [HttpGet("blog")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("blog/{id}")]
        public IActionResult Detail(int id)
        {
            return View();
        }
    }
}