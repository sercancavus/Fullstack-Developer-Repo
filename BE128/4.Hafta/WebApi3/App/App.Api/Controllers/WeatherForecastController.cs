using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{

    // Controller: HTTP isteklerini kar��layan C# classlar�d�r.
    // Controllerlar standart C# classlar�d�r. Fakat standart classlara g�re bir tak�m ekstra yetenekleri vard�r.

    // Bir class�n controller olabilmesi i�in gereken �artlar :
    // ------------------------------------------------------------------------

    // 1) ControllerBase class�ndan inherit(kal�t�m) almas� gerekir.
    // 2) [ApiController] attribute'u ile nitelenmi�(i�aretlenmi�) olmas� gerekir.
    // 3) �sminin sonu "Controller" ile bitmesi gerekir.
    // 4) Proje i�erisindeki "Controllers" klas�r� i�erisinde bulunmas� gerekir.

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
