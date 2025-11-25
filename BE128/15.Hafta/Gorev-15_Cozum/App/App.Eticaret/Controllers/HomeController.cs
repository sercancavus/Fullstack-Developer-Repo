using App.Data.Entities;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    public class HomeController(IHttpClientFactory clientFactory) : BaseController
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

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

            var contactMessageEntity = new ContactFormEntity
            {
                Name = newContactMessage.Name,
                Email = newContactMessage.Email,
                Message = newContactMessage.Message,
                SeenAt = null
            };

            var response = await Client.PostAsJsonAsync("/contact-form", contactMessageEntity);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "An error occurred while sending your message. Please try again later.";
                return View(newContactMessage);
            }

            SetSuccessMessage("Your message has been sent successfully.");

            return View();
        }

        [HttpGet("/product/list")]
        public async Task<IActionResult> Listing()
        {
            // TODO: add paging support

            var response = await Client.GetAsync("/products");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var products = await response.Content.ReadFromJsonAsync<List<ProductListingViewModel>>();

            return View(products);
        }

        [HttpGet("/product/{productId:int}/details")]
        public async Task<IActionResult> ProductDetail([FromRoute] int productId)
        {

            var response = await Client.GetAsync($"/products/{productId}/home");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var product = await response.Content.ReadFromJsonAsync<HomeProductDetailViewModel>();

            if (product is null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}