using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    [Authorize(Roles = "seller, buyer")]
    public class ProfileController(IHttpClientFactory clientFactory) : BaseController
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

        [HttpGet("/profile")]
        public async Task<IActionResult> Details()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await Client.GetAsync($"/user/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userViewModel = await response.Content.ReadFromJsonAsync<ProfileDetailsViewModel>();

            if (userViewModel is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string? previousSuccessMessage = TempData["SuccessMessage"] as string;

            if (previousSuccessMessage is not null)
            {
                SetSuccessMessage(previousSuccessMessage);
            }

            return View(userViewModel);
        }

        [HttpPost("/profile")]
        public async Task<IActionResult> Edit([FromForm] ProfileDetailsViewModel editMyProfileModel)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await GetCurrentUserAsync();

            if (user is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(editMyProfileModel);
            }

            user.FirstName = editMyProfileModel.FirstName;
            user.LastName = editMyProfileModel.LastName;

            if (!string.IsNullOrWhiteSpace(editMyProfileModel.Password) && editMyProfileModel.Password != "******")
            {
                user.Password = editMyProfileModel.Password;
            }

            var response = await Client.PutAsJsonAsync($"/user/{user.Id}", user);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Bir hata oluştu. Lütfen tekrar deneyin.");
                return View(editMyProfileModel);
            }

            TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";

            return RedirectToAction(nameof(Details));
        }

        [HttpGet("/my-orders")]
        public async Task<IActionResult> MyOrders()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await Client.GetAsync($"/user/{userId}/orders");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Auth");
            }

            var orders = await response.Content.ReadFromJsonAsync<List<OrderViewModel>>();

            return View(orders ?? []);
        }

        [HttpGet("/my-products")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> MyProducts()
        {
            var userId = GetUserId();

            if (userId is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var response = await Client.GetAsync($"/products?sellerId={userId}");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login", "Auth");
            }

            var products = await response.Content.ReadFromJsonAsync<List<MyProductsViewModel>>();

            return View(products ?? []);
        }
    }
}