using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{

    // Controller: HTTP isteklerini karþýlayan C# classlarýdýr.
    // Controllerlar standart C# classlarýdýr. Fakat standart classlara göre bir takým ekstra yetenekleri vardýr.

    // Bir classýn controller olabilmesi için gereken þartlar :
    // ------------------------------------------------------------------------

    // 1) ControllerBase classýndan inherit(kalýtým) almasý gerekir.
    // 2) [ApiController] attribute'u ile nitelenmiþ(iþaretlenmiþ) olmasý gerekir.
    // 3) Ýsminin sonu "Controller" ile bitmesi gerekir.
    // 4) Proje içerisindeki "Controllers" klasörü içerisinde bulunmasý gerekir.

    [ApiController]
    //[Route("[controller]")]
    //[Route("[HavaDurumu]")]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
