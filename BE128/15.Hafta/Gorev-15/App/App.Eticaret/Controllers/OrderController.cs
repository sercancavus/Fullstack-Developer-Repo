using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Eticaret.Controllers
{
    [Authorize(Roles = "buyer, seller")]
    public class OrderController : BaseController
    {
        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private readonly IHttpClientFactory _httpClientFactory;

        [HttpPost("/order")]
        public async Task<IActionResult> Create([FromForm] object model)
        {
            // TODO: Migrate order creation to Data API (cart -> order)
            return RedirectToAction(nameof(CartController.Edit), "Cart");
        }

        [HttpGet("/order/{orderCode}/details")]
        public async Task<IActionResult> Details([FromRoute] string orderCode)
        {
            var client = _httpClientFactory.CreateClient("DataApi");
            var order = await client.GetFromJsonAsync<OrderDetailsViewModel>($"/api/order/{orderCode}/details");
            if (order is null)
            {
                return NotFound();
            }
            return View(order);
        }

        private async Task<string> CreateOrderCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 10).ToUpperInvariant();
        }
    }
}