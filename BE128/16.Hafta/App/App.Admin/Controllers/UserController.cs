using App.Admin.Models.ViewModels;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("/users")]
    public class UserController(IHttpClientFactory clientFactory) : Controller
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await Client.GetAsync("api/user");

            List<UserListItemViewModel> users = [];

            if (!response.IsSuccessStatusCode)
            {
                return View(users);
            }

            users = (await response.Content.ReadFromJsonAsync<List<UserEntity>>())?.Select(u => new UserListItemViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role.Name,
                Enabled = u.Enabled,
                HasSellerRequest = u.HasSellerRequest
            }).ToList()
                ?? [];

            return View(users);
        }

        [Route("/users/{id:int}/approve")]
        [HttpGet]
        public async Task<IActionResult> ApproveSellerRequest([FromRoute] int id)
        {
            var response = await Client.GetAsync($"api/user/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var user = await response.Content.ReadFromJsonAsync<UserEntity>();

            if (user == null)
            {
                return NotFound();
            }

            if (!user.HasSellerRequest)
            {
                return BadRequest();
            }

            user.HasSellerRequest = false;
            user.RoleId = 2; // seller

            var updateResponse = await Client.PutAsJsonAsync($"api/user/{id}", user);

            if (!updateResponse.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(List));
        }

        [Route("/users/{id:int}/enable")]
        public async Task<IActionResult> Enable([FromRoute] int id)
        {
            var response = await Client.GetAsync($"api/user/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var user = await response.Content.ReadFromJsonAsync<UserEntity>();

            if (user is null)
            {
                return NotFound();
            }

            user.Enabled = true;

            var updateResponse = await Client.PutAsJsonAsync($"api/user/{id}", user);

            if (!updateResponse.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(List));
        }

        [Route("/users/{id:int}/disable")]
        public async Task<IActionResult> Disable([FromRoute] int id)
        {
            var response = await Client.GetAsync($"api/user/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var user = await response.Content.ReadFromJsonAsync<UserEntity>();

            if (user == null)
            {
                return NotFound();
            }

            user.Enabled = false;

            var updateResponse = await Client.PutAsJsonAsync($"api/user/{id}", user);

            if (!updateResponse.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(List));
        }
    }
}