using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonController : ControllerBase
    {
        [HttpGet("ok-json")]
        public IActionResult Get()
        {
            return Ok(new { Name = "Ali", Age = 30, Address = "İstanbul" });
        }

        [HttpGet("array-to-json")]
        public IActionResult GetArray()
        {
            string[] names = new string[] { "Ali", "Veli", "Ayşe" };
            return Ok(names);
        }

        [HttpGet("list-json")]
        public IActionResult GetList()
        {
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 10000 },
                new Product { Name = "Monitor", Price = 5000 },
                new Product { Name = "Mouse", Price = 1000 }
            };
            return Ok(products);

            // Hyper Text Transfer Protocol : HTTP

            //Serialize işlemi : bir objecti json formatında bir stringe çevirmek

            string jsonString = JsonSerializer.Serialize(products);

            return Content(jsonString, "text/plain;charset=utf-8");
        }

        [HttpGet("list-to-json-and-back")]
        public IActionResult GetListAndBack()
        {
            var products = new List<Product>
            {
                new Product { Name = "Laptop", Price = 10000 },
                new Product { Name = "Monitor", Price = 5000 },
                new Product { Name = "Mouse", Price = 1000 }
            };
            // Serialize işlemi
            string jsonString = JsonSerializer.Serialize(products);
            // Deserialize işlemi
            var deserializedProducts = JsonSerializer.Deserialize<List<Product>>(jsonString);
            return Ok(deserializedProducts);
        }

        [HttpGet("list-with-result")]
        public IResult GetPersonWithIResult()
        {
            // “IResult” kullanarak JSON response oluşturmak:

            // “StatusCodes.Status200OK” ifadesi int tipinde 200 değerini verir,
            // 200 değerini yazmaktansa bu ifadeyi kullanmak
            // okunabilirliği artırır, hata yapma olasılığını düşürür

            List<Product> products = new List<Product>();
            {
                products.Add(new Product { Name = "Laptop", Price = 10000 });
                products.Add(new Product { Name = "Monitor", Price = 5000 });
                products.Add(new Product { Name = "Mouse", Price = 1000 });
            }
            ;
            return Results.Json(products, statusCode: StatusCodes.Status200OK);
        }

        [HttpGet("list-with-result-2")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IResult GetPersonWithIResult2()
        {
            List<Product> products = new List<Product>();
            {
                products.Add(new Product { Name = "Laptop", Price = 10000 });
                products.Add(new Product { Name = "Monitor", Price = 5000 });
                products.Add(new Product { Name = "Mouse", Price = 1000 });

                if (products.Count == 0)
                {
                    return Results.NotFound("liste boş");
                }

                if (products.Count!= 3)
                                    {
                    return Results.BadRequest("liste 3 eleman içermeli");
                }
            }

            return Results.Json(products, statusCode: StatusCodes.Status200OK);
        }

        // Ürün fiyatının dolar olarak girildiği kabul edilerek; 
        // 1) Request body'den bir product gönderilecek ("application/json")
        // 2) Request'ten gelen bu product ilgili action içerisinde
        //    - fiyatı 0'dan küçükse "Fiyat 0'dan küçük olamaz" mesajı ile BadRequest olarak dönecek
        //    - değilse, fiyat dolar'dan tl'te çevrilerek güncellenecek ve Ok ile response olarak döndürülecek

        [HttpPost("convert-price")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public IActionResult ConvertPrice(Product product)
            
        {
            if (product.Price < 0)
            {
                return BadRequest("Fiyat 0'dan küçük olamaz");
            }
            // Dolar'dan TL'ye çevirme işlemi (örnek kur: 1 Dolar = 40,66 TL)
            decimal exchangeRate = 40.66m;
            product.Price *= exchangeRate;
            return Ok(product);
        }


        public class Product
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
    }
}
