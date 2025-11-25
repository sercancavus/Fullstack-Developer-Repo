using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        [Route("/products/")]
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

        [Route("/products/filter")]
        [HttpGet]
        public IActionResult Filter([FromQuery] object filterOptions)
        {
            // will return filtered products as json
            return Json(new { });
        }

        [Route("/products/{productId:int}/delete")]
        [HttpGet]
        public IActionResult Delete([FromRoute] int productId)
        {
            return View();
        }
    }
}