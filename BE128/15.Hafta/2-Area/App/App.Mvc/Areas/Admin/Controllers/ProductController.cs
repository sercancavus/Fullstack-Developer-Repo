using App.Mvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View(ProductList.Products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] string productName)
        {
            ProductList.Products.Add(productName);
            return RedirectToAction("Index");
        }

        public IActionResult Delete([FromRoute] int id)
        {
            ProductList.Products.RemoveAt(id);
            return RedirectToAction("Index");
        }
    }
}
