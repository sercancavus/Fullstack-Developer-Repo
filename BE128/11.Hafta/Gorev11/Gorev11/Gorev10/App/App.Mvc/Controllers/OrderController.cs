using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class OrderController : Controller
    {
        [Route("order/{id}/details")]
        public IActionResult Details(int id)
        {
            // Sipariþ detaylarý için id parametresi alýnýr
            return View();
        }
    }
}