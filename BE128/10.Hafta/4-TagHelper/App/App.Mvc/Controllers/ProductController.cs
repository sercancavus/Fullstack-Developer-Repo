using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet("product")]
        public IActionResult Index()
        {
            ViewBag.Welcome = "Ürün ekleme ekranına hoşgeldiniz";

            return View();
        }
        [HttpPost("product")]
        public IActionResult Index([FromForm] ProductViewModel product)
        {
            ViewBag.Welcome = null;

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Ürün eklenmedi";
                return View();
            }

            ViewBag.SuccessMessage = "Ürün başarıyla eklendi";

            return View();
        }
    }
}
