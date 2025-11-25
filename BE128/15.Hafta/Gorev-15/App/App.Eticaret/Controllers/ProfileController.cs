using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Eticaret.Controllers
{
    [Authorize(Roles = "seller, buyer")]
    public class ProfileController(IHttpClientFactory httpClientFactory) : BaseController
    {
        [HttpGet("/profile")]
        public async Task<IActionResult> Details()
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var userViewModel = await client.GetFromJsonAsync<ProfileDetailsViewModel>("/api/profile/me");
            if (userViewModel is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string? previousSuccessMessage = TempData["SuccessMessage"]?.ToString();
            if (previousSuccessMessage is not null)
            {
                SetSuccessMessage(previousSuccessMessage);
            }

            return View(userViewModel);
        }

        public record UpdateProfileRequest(string FirstName, string LastName, string? Password);

        [HttpPost("/profile")]
        public async Task<IActionResult> Edit([FromForm] ProfileDetailsViewModel editMyProfileModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editMyProfileModel);
            }

            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PutAsJsonAsync("/api/profile", new UpdateProfileRequest(
                editMyProfileModel.FirstName,
                editMyProfileModel.LastName,
                !string.IsNullOrWhiteSpace(editMyProfileModel.Password) && editMyProfileModel.Password != "******"
                    ? editMyProfileModel.Password
                    : null));

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Profil güncellenemedi.");
                return View(editMyProfileModel);
            }

            TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction(nameof(Details));
        }

        [HttpGet("/my-orders")]
        public async Task<IActionResult> MyOrders()
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var orders = await client.GetFromJsonAsync<List<OrderViewModel>>("/api/order/my");
            return View(orders ?? new List<OrderViewModel>());
        }

        [HttpGet("/my-products")]
        [Authorize(Roles = "seller")]
        public async Task<IActionResult> MyProducts()
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var list = await client.GetFromJsonAsync<List<MyProductsViewModel>>("/api/product/list");
            // If product list endpoint returns all, filter client-side for own items is not ideal; would add dedicated endpoint later.
            return View(list ?? new List<MyProductsViewModel>());
        }
    }
}