using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutputsController : ControllerBase
    {
        // 1) https://www.siliconmadeacademy.com  adresine yönlendirme yapan bir endpoint oluşturun 
        // 2) 1. action'a yönlendirme yapan bir endpoint oluşturun 
        // 3) Bir product nesnesini 200 status kodu ile döndüren bir endpoint oluşturun 
        // 4) İsminizi italik olarak yazdıran bir endpoint oluşturun

        [HttpGet("redirect-to-siliconmadeacademy")]
        public IActionResult RedirectToSiliconMadeAcademy()
        {
            return Redirect("https://www.siliconmadeacademy.com");
        }
        [HttpGet("redirect-to-previous")]
        public IActionResult RedirectToPrevious()
        {
            return RedirectToAction(nameof(RedirectToSiliconMadeAcademy));
        }
        //[HttpGet("product")]
        //public IActionResult GetProduct()
        //{
        //    var product = new
        //    {
        //        Id = 1,
        //        Name = "Sample Product",
        //        Price = 19.99
        //    };
        //    return Ok(product);
        //}

        [HttpGet("product")]
        public IActionResult Product()
        { //Initialize etmek
            var product = new Product
                {
                Name = "Sample Product",
                Price = 19.99m,
                Description = "This is a sample product description."
                };
            return Ok(product);
        }


        [HttpGet("name-italic")]
        public IActionResult GetNameItalic()
        {
            return Content("<i>John Doe</i>", "text/html");
        }
        // 5) Bir hata mesajı ile 404 status kodu döndüren bir endpoint oluşturun
        [HttpGet("not-found")]
        public IActionResult NotFoundError()
        {
            return NotFound("The requested resource was not found.");
        }

    }
}
