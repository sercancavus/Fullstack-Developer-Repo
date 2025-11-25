using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("/about-us")]
        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("/contact")]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [Route("/contact")]
        [HttpPost]
        public IActionResult Contact([FromForm] object newContactMessageModel)
        {
            return View();
        }

        [Route("/product/list")]
        [HttpGet]
        public IActionResult Listing()
        {
            return View();
        }

        [Route("/product/{productId:int}/details")]
        [HttpGet]
        public IActionResult ProductDetail([FromRoute] int productId)
        {
            return View();
        }
    }
}