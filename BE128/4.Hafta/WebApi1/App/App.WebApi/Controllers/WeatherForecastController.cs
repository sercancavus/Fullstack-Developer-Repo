using Microsoft.AspNetCore.Mvc;

namespace App.WebApi.Controllers

// CONTROLLER : HTTP requestlerini kar��layan C# classlar�d�r.



    // C# ATTRUBUTE'lar�
    // Attributelar alt�ndaki (attribute olmayan) ilk yap�y� nitelemek ve ona bir tak�m �zellikler kazand�rmak i�in kullan�l�r.

{
    [ApiController] // WeatherForecastController'�n bir api Controller'� oldu�unu belirtir.
    [Route("[controller]")] // Alt�ndaki WeatherForecastController'�n route'unu belirlemek i�in kullan�l�r.
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
