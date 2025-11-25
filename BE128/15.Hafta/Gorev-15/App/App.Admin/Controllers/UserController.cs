using App.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController(IHttpClientFactory httpClientFactory) : Controller
    {
        [Route("/users")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var users = await client.GetFromJsonAsync<List<UserListItemViewModel>>("/api/admin/users");
            return View(users ?? new List<UserListItemViewModel>());
        }

        [Route("/users/{id:int}/approve")]
        [HttpGet]
        public async Task<IActionResult> ApproveSellerRequest([FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsync($"/api/admin/users/{id}/approve-seller", null);
            if (!response.IsSuccessStatusCode) return BadRequest();
            return RedirectToAction(nameof(List));
        }

        [Route("/users/{id:int}/enable")]
        public async Task<IActionResult> Enable([FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsync($"/api/admin/users/{id}/enable", null);
            if (!response.IsSuccessStatusCode) return NotFound();
            return RedirectToAction(nameof(List));
        }

        [Route("/users/{id:int}/disable")]
        public async Task<IActionResult> Disable([FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsync($"/api/admin/users/{id}/disable", null);
            if (!response.IsSuccessStatusCode) return NotFound();
            return RedirectToAction(nameof(List));
        }
    }
}