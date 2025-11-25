using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    public class ProductController : Controller
    {
        [Route("/product")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("/product")]
        [HttpPost]
        public IActionResult Create([FromForm] object newProductModel)
        {
            return View();
        }

        [Route("/product/{productId:int}/edit")]
        [HttpGet]
        public IActionResult Edit([FromRoute] int productId)
        {
            return View();
        }

        [Route("/product/{productId:int}/edit")]
        [HttpPost]
        public IActionResult Edit([FromRoute] int productId, [FromForm] object editProductModel)
        {
            return View();
        }

        [Route("/product/{productId:int}/delete")]
        [HttpGet]
        public IActionResult Delete([FromRoute] int productId)
        {
            return View();
        }

        [Route("/product/{productId:int}/comment")]
        [HttpPost]
        public IActionResult Comment([FromRoute] int productId, [FromForm] object newProductCommentModel)
        {
            // save product comment...

            return RedirectToAction(nameof(HomeController.ProductDetail), "Home", new { productId });
        }
    }
}