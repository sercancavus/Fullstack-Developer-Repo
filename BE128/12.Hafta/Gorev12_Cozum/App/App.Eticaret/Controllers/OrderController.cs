using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    public class OrderController : Controller
    {
        [Route("/order")]
        [HttpPost]
        public IActionResult Create()
        {
            // create order...
            var orderId = 1;
            return RedirectToAction(nameof(Details), new { orderId });
        }

        [Route("/order/{orderId:int}/details")]
        [HttpGet]
        public IActionResult Details([FromRoute] int orderId)
        {
            return View();
        }
    }
}