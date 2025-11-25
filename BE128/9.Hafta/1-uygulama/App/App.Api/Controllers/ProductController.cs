using App.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static List<Product> _products = new()
        {
            new Product {Id = 1, Name = "Urun1" , Description = "Açıklama1", Price = 1000, Quantity = 10, Image = "Image1.png" },
            new Product {Id = 2, Name = "Urun2" , Description = "Açıklama2", Price = 2000, Quantity = 20, Image = "Image2.png" },
            new Product {Id = 3, Name = "Urun3" , Description = "Açıklama3", Price = 3000, Quantity = 30, Image = "Image3.png" },
        };


        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_products);
        }



    }
}
