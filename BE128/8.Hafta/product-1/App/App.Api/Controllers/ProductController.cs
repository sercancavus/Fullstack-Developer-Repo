using App.Api.Data;
using App.Api.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private AppDbContext Context { get; set; }
        public ProductController()
        {
            Context = new AppDbContext();
            Context.Database.EnsureCreated();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = Context.Products.ToList();

            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var product = Context.Products.FirstOrDefault(x => x.Id == id);

            return Ok(product);
        }
        [HttpPost]
        public IActionResult Create([FromBody] ProductEntity product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Context.Products.Add(product);

            Context.SaveChanges();

            return Ok();
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] ProductEntity product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbProduct = Context.Products.FirstOrDefault(x => x.Id == id);

            if (dbProduct is null)
            {
                return NotFound();
            }

            dbProduct.Name = product.Name;
            dbProduct.Description = product.Description;
            dbProduct.Stock = product.Stock;

            Context.SaveChanges();

            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var dbProduct = Context.Products.FirstOrDefault(x => x.Id == id);

            if (dbProduct is null)
            {
                return NotFound();
            }

            Context.Products.Remove(dbProduct);
            Context.SaveChanges();

            return Ok();
        }

    }
}
