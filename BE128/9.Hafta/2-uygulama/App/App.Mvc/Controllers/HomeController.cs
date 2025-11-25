using System.Diagnostics;
using System.Threading.Tasks;
using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Product>? products = await GetProductsFromApi();
            ViewData["Product"] = products;

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

        public async Task<List<Product>?> GetProductsFromApi()
        {
            string url = "https://localhost:7201/api/Product";

            // Http istekleri yapmamýzý saðlar. (fetch gibi)
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url); // get isteði yapýlýr

            if (response.IsSuccessStatusCode)
            {
                List<Product>? products = await response.Content.ReadFromJsonAsync<List<Product>>();

                return products;
            }
            else
            {
                return null;
            }
        }

    }
}
