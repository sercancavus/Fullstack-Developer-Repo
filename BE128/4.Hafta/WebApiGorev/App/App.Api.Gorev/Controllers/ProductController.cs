using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Gorev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        // Burada ürünler için CRUD metotları yazılacak, 5 ürün olabilir, static bir liste kullanılabilir.

        [HttpGet]
        public IActionResult GetProducts()
        {
            // Türkçe ürün listesi
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Kavun", Description = "Tatlı ve sulu kavun", Price = 35.0m, Category = "Meyve" },
                new Product { Id = 2, Name = "Karpuz", Description = "Yazın serinleten karpuz", Price = 25.0m, Category = "Meyve" },
                new Product { Id = 3, Name = "Peynir", Description = "Taze beyaz peynir", Price = 80.0m, Category = "Süt Ürünü" },
                new Product { Id = 4, Name = "Üzüm", Description = "Çekirdeksiz yeşil üzüm", Price = 40.0m, Category = "Meyve" },
                new Product { Id = 5, Name = "Erik", Description = "Ekşi yeşil erik", Price = 60.0m, Category = "Meyve" }
            };
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult Index(int id) {
            // Türkçe ürün listesi
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Kavun", Description = "Tatlı ve sulu kavun", Price = 35.0m, Category = "Meyve" },
                new Product { Id = 2, Name = "Karpuz", Description = "Yazın serinleten karpuz", Price = 25.0m, Category = "Meyve" },
                new Product { Id = 3, Name = "Peynir", Description = "Taze beyaz peynir", Price = 80.0m, Category = "Süt Ürünü" },
                new Product { Id = 4, Name = "Üzüm", Description = "Çekirdeksiz yeşil üzüm", Price = 40.0m, Category = "Meyve" },
                new Product { Id = 5, Name = "Erik", Description = "Ekşi yeşil erik", Price = 60.0m, Category = "Meyve" }
            };
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null");
            }
            // Burada ürün ekleme işlemi yapılacak
            // Örnek olarak, ürünü listeye ekleyebiliriz
            return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null || product.Id != id)
            {
                return BadRequest("Product ID mismatch");
            }
            // Burada ürün güncelleme işlemi yapılacak
            // Örnek olarak, ürünü liste üzerinde güncelleyebiliriz
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            // Burada ürün silme işlemi yapılacak
            // Örnek olarak, ürünü liste üzerinden silebiliriz
            return NoContent();
        }

    }
}
