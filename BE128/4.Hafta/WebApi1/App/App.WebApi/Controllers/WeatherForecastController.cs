using Microsoft.AspNetCore.Mvc;

namespace App.WebApi.Controllers

// CONTROLLER : HTTP requestlerini karþýlayan C# classlarýdýr.



    // C# ATTRUBUTE'larý
    // Attributelar altýndaki (attribute olmayan) ilk yapýyý nitelemek ve ona bir takým özellikler kazandýrmak için kullanýlýr.

{
    [ApiController] // WeatherForecastController'ýn bir api Controller'ý olduðunu belirtir.
    [Route("[controller]")] // Altýndaki WeatherForecastController'ýn route'unu belirlemek için kullanýlýr.
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //private readonly ILogger<WeatherForecastController> _logger;

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet]
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
