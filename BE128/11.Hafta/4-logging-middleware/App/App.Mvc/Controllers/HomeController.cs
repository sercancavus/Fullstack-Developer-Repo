using System.Diagnostics;
using App.Mvc.Models;
using App.Mvc.Models.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IOptions<UserConfigModel> _userConfig;


        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IOptions<UserConfigModel> userConfig)
        {
            _logger = logger;
            _configuration = configuration;
            _userConfig = userConfig;
        }

        public IActionResult Index()
        {
            // 1)
            //_logger.LogInformation("anasayfaya istekte bulunuldu.");

            // 2)
            //_logger.LogInformation("{tarih} - anasayfaya istekte bulunuldu.", DateTime.Now);

            // 3)
            //_logger.LogWarning("{tarih} - anasayfaya istekte bulunuldu.", DateTime.Now);

            // 4) 
            //_logger.LogError("{tarih} - anasayfaya istekte bulunuldu.", DateTime.Now);

            // 5)
            _logger.LogInformation("{tarih} - anasayfaya istekte bulunuldu.", DateTime.Now);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("home/config/{key}")]
        public IActionResult ConfigValue([FromRoute] string key)
        {
            //var v = _configuration.GetValue<string>(key);


            var v = _configuration.GetValue<int>(key);

            return Ok(new {value = v} );
        }

        [HttpGet("home/user")]
        public IActionResult CustomUser()
        {
            return Ok(_userConfig.Value);
        }

    }
}
