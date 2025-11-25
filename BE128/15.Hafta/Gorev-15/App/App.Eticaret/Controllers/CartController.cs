using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Eticaret.Controllers
{
    [Authorize(Roles = "buyer, seller")]
    public class CartController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CartController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("/add-to-cart/{productId:int}")]
        public async Task<IActionResult> AddProduct([FromRoute] int productId)
        {
            var client = _httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsync($"/api/cart/add/{productId}", null);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            var prevUrl = Request.Headers.Referer.FirstOrDefault();
            return prevUrl is null ? RedirectToAction(nameof(Edit)) : Redirect(prevUrl);
        }

        [HttpGet("/cart")]
        public async Task<IActionResult> Edit()
        {
            var client = _httpClientFactory.CreateClient("DataApi");
            var items = await client.GetFromJsonAsync<List<CartItemViewModel>>("/api/cart/my");
            return View(items ?? new List<CartItemViewModel>());
        }

        [HttpGet("/cart/{cartItemId:int}/remove")]
        public async Task<IActionResult> Remove([FromRoute] int cartItemId)
        {
            var client = _httpClientFactory.CreateClient("DataApi");
            var response = await client.DeleteAsync($"/api/cart/{cartItemId}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Edit));
        }

        public record UpdateCartRequest(byte Quantity);

        [HttpPost("/cart/update")]
        public async Task<IActionResult> UpdateCart(int cartItemId, byte quantity)
        {
            var client = _httpClientFactory.CreateClient("DataApi");
            var response = await client.PutAsJsonAsync($"/api/cart/{cartItemId}", new UpdateCartRequest(quantity));
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var model = await response.Content.ReadFromJsonAsync<CartItemViewModel>();
            return View(model);
        }

        [HttpGet("/checkout")]
        public async Task<IActionResult> Checkout()
        {
            var client = _httpClientFactory.CreateClient("DataApi");
            var cartItems = await client.GetFromJsonAsync<List<CartItemViewModel>>("/api/cart/my");
            return View(cartItems ?? new List<CartItemViewModel>());
        }
    }
}