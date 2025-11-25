using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class UrunController : Controller
    {
        private static List<Product> products = new()
        {
            new Product { Id = 1, Name = "Laptop", Description = "Gaming Laptop", Price =2500, Quantity = 5, Image =  "laptop.jpg" },
            new Product { Id = 2, Name = "Mouse", Description = "Wireless Mouse", Price = 50 ,Quantity =20, Image = "mouse.jpg" },
            new Product { Id = 3, Name = "Keyboard", Description = "Mechanical Keyboard", Price = 1500,Quantity = 10, Image ="keyboard.jpg" },
            new Product { Id = 4, Name = "Monitor", Description = "4K Monitor", Price = 7000,Quantity =7, Image = "monitor.jpg" },
            new Product { Id = 5, Name = "Headset", Description = "Bluetooth Headset",Price=1200,Quantity = 15, Image ="headset.jpg" }
        };

        [HttpGet]
        public IActionResult List()
        {
            ViewBag.Urunler = products;
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromForm] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            products.Add(product);

            return RedirectToAction(nameof(List));
        }

    }
}
