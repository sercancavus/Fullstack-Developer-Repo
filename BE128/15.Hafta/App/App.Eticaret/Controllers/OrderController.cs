using App.Eticaret.Models;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    [Authorize(Roles = "buyer, seller")]
    public class OrderController(IHttpClientFactory clientFactory) : BaseController
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

        [HttpPost("/order")]
        public async Task<IActionResult> Create([FromForm] CheckoutViewModel model)
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            if (!ModelState.IsValid)
            {
                var viewModel = await GetCartItemsAsync();
                return View(viewModel);
            }

            var response = await Client.PostAsJsonAsync("cart/checkout", new CheckoutRequest
            {
                UserId = userId.Value,
                Address = model.Address,
            });

            if (!response.IsSuccessStatusCode)
            {
                return CheckoutFailedResult(model, response);
            }

            var order = await response.Content.ReadFromJsonAsync<OrderDetailsViewModel>();

            if (order is null)
            {
                return CheckoutFailedResult(model, response);
            }

            return RedirectToAction(nameof(Details), new { orderCode = order.OrderCode });
        }

        private IActionResult CheckoutFailedResult(CheckoutViewModel model, HttpResponseMessage response)
        {
            // TODO: create a view model for error messages
            throw new NotImplementedException();
        }

        [HttpGet("/order/{orderCode}/details")]
        public async Task<IActionResult> Details([FromRoute] string orderCode)
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction(nameof(AuthController.Login), "Auth");
            }

            var order = await Client.GetFromJsonAsync<OrderDetailsViewModel>($"order/{userId}/{orderCode}");

            if (order is null)
            {
                return NotFound();
            }

            return View(order);
        }

        private async Task<List<CartItemViewModel>> GetCartItemsAsync()
        {
            var userId = GetUserId() ?? -1;

            var response = await Client.GetAsync($"cart/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return [];
            }

            var cartItems = await response.Content.ReadFromJsonAsync<List<CartItemViewModel>>();

            return cartItems ?? [];
        }
    }
}