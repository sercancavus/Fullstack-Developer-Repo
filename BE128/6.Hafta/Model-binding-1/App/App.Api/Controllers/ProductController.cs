using App.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    // Model Binding : Bir controller'a istekte bulunulduğunda, verilerin request'in herhangi bir yerinden (request body, route, query string, form verileri) alınıp uygulama içerisinde ilgili modeli doldurma(modele atama) işidir.

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static List<Product> products = new List<Product>()
        {
            new Product { Id = 1, Name = "Product1", Price = 5},
            new Product { Id = 2, Name = "Product2", Price = 15},
            new Product { Id = 3, Name = "Product3", Price = 25},
            new Product { Id = 4, Name = "Product4", Price = 35},
            new Product { Id = 5, Name = "Product5", Price = 45}
        };

        [HttpPost("add")]
        public IActionResult Post([FromBody] Product product)
        {
            products.Add(product);
            return Ok();
        }

        [HttpGet("get1/{id}")]
        public IActionResult Get1([FromRoute] int id)
        {
            var product = products.Find(x => x.Id == id);

            if (product == null)
            {
                return NotFound(); // early return
            }

            return Ok(product);
        }

        [HttpGet("get2")]
        public IActionResult Get2([FromQuery] int id)
        {
            var product = products.Find(x => x.Id == id);

            if (product == null)
            {
                return NotFound(); // early return
            }

            return Ok(product);
        }

        [HttpPost("form")]
        public IActionResult Form([FromForm] Product product)
        {
            products.Add(product);
            return Ok();
        }

        // Bir action içerisinde 3 adet int parametre alınacak,
        // 1 parametre Route'dan
        // 1 parametre Query'den
        // 1 parametre Header'dan
        // (Route'dan alınan değer - Query'den Alınan değer) * Header'dan alınan değer işlemi yapılacak
        // Sonuç Ok ile döndürülecek
        // İstek postman ile denenecek


    }
}
