using App.Mvc.Data;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View(ProductList.Products);
        }
        public IActionResult Buy()
        {
            ViewBag.Buy = "Satın Alma İşlemi Başarılı";
            return View();
        }
    }
}
