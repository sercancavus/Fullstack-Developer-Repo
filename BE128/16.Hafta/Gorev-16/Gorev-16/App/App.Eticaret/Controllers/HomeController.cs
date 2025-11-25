using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Eticaret.Controllers
{
    public class HomeController(IHttpClientFactory httpClientFactory) : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet("/contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("/contact")]
        public async Task<IActionResult> Contact([FromForm] NewContactFormMessageViewModel newContactMessage)
        {
            if (!ModelState.IsValid)
            {
                return View(newContactMessage);
            }

            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsJsonAsync("/api/contact", new
            {
                name = newContactMessage.Name,
                email = newContactMessage.Email,
                message = newContactMessage.Message
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Mesaj gönderilemedi.");
                return View(newContactMessage);
            }

            ViewBag.SuccessMessage = "Your message has been sent successfully.";

            return View();
        }

        [HttpGet("/product/list")]
        public async Task<IActionResult> Listing()
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var products = await client.GetFromJsonAsync<List<ProductListingViewModel>>("/api/product/list");
            return View(products ?? new List<ProductListingViewModel>());
        }

        [HttpGet("/product/{productId:int}/details")]
        public async Task<IActionResult> ProductDetail([FromRoute] int productId)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var product = await client.GetFromJsonAsync<HomeProductDetailViewModel>($"/api/product/{productId}");
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}