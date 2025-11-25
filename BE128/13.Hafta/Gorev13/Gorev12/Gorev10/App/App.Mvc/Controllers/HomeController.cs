using System.Diagnostics;
using App.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() => View();

        [Route("about-us")]
        [AllowAnonymous]
        public IActionResult AboutUs() => View();

        [Route("contact")]
        [AllowAnonymous]
        public IActionResult Contact() => View();

        [Route("listing")]
        public IActionResult Listing()
        {
            // Anasayfa benzeri ürünler listesi
            var products = StaticProducts();
            return View(products);
        }

        [Route("product/{categoryName}/{title}/{id}")]
        public IActionResult ProductDetail(string categoryName, string title, int id)
        {
            var products = StaticProducts();
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        private List<ProductDetailViewModel> StaticProducts() => new()
        {
            new ProductDetailViewModel{ Id=1, Name="Buzdolabý", Category="Beyaz Eþya", Price=15000, Stock=72, Description="Buzdolabý açýklamasý", Slug="buzdolabi"},
            new ProductDetailViewModel{ Id=2, Name="Çamaþýr Makinesi", Category="Beyaz Eþya", Price=12000, Stock=85, Description="Çamaþýr makinesi açýklamasý", Slug="camasir-makinesi"},
            new ProductDetailViewModel{ Id=3, Name="Koltuk Takýmý", Category="Mobilya", Price=25000, Stock=60, Description="Koltuk takýmý açýklamasý", Slug="koltuk-takimi"},
            new ProductDetailViewModel{ Id=4, Name="Yatak", Category="Mobilya", Price=8000, Stock=74, Description="Yatak açýklamasý", Slug="yatak"},
            new ProductDetailViewModel{ Id=5, Name="Akýllý Telefon", Category="Akýllý Cihaz", Price=20000, Stock=95, Description="Akýllý telefon açýklamasý", Slug="akilli-telefon"},
            new ProductDetailViewModel{ Id=6, Name="Akýllý Saat", Category="Akýllý Cihaz", Price=5000, Stock=88, Description="Akýllý saat açýklamasý", Slug="akilli-saat"},
            new ProductDetailViewModel{ Id=7, Name="Televizyon", Category="Eðlence", Price=18000, Stock=53, Description="Televizyon açýklamasý", Slug="televizyon"},
            new ProductDetailViewModel{ Id=8, Name="Oyun Konsolu", Category="Eðlence", Price=12000, Stock=77, Description="Oyun konsolu açýklamasý", Slug="oyun-konsolu"},
            new ProductDetailViewModel{ Id=9, Name="Bluetooth Kulaklýk", Category="Eðlence", Price=2000, Stock=64, Description="Bluetooth kulaklýk açýklamasý", Slug="bluetooth-kulaklik"},
            new ProductDetailViewModel{ Id=10, Name="Kahve Makinesi", Category="Küçük Ev Aleti", Price=3500, Stock=69, Description="Kahve makinesi açýklamasý", Slug="kahve-makinesi"}
        };

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
