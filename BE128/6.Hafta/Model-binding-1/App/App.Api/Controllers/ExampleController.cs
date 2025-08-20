using App.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        // Bir action içerisinde 3 adet int parametre alınacak,
        // 1 parametre Route'dan
        // 1 parametre Query'den
        // 1 parametre Header'dan
        // (Route'dan alınan değer - Query'den Alınan değer) * Header'dan alınan değer işlemi yapılacak
        // Sonuç Ok ile döndürülecek
        // İstek postman ile denenecek

        [HttpGet("get/{sayi1}")]
        public IActionResult Get([FromRoute] int sayi1,[FromQuery] int sayi2,[FromHeader] int sayi3)
        {
            var result = (sayi1 - sayi2) * sayi3;

            return Ok(result);
        }

        [HttpGet("get2/{sayi1}")]
        public IActionResult Get2(ExampleModel model)
        {
            var result = (model.Sayi1 - model.Sayi2) * model.Sayi3;

            return Ok(result);
        }

        [HttpGet("get3/{sayi1}/{sayi2}")]
        public IActionResult Get3(ExampleModelWithBindings model)
        {
            model.Sonuc = model.Sayi1 * model.Sayi2;
            return Ok(model);
        }

    }
}
