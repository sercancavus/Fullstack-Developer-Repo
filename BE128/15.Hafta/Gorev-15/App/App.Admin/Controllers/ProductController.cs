using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController(IHttpClientFactory httpClientFactory) : Controller
    {
        public record AdminProductItem(int Id, string Name, decimal Price, bool Enabled, string Seller);

        [Route("/products/")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var items = await client.GetFromJsonAsync<List<AdminProductItem>>("/api/admin/products");
            return View(items ?? new List<AdminProductItem>());
        }

        [Route("/products/{productId:int}/disable")]
        [HttpGet]
        public async Task<IActionResult> Disable([FromRoute] int productId)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsync($"/api/admin/products/{productId}/disable", null);
            if (!response.IsSuccessStatusCode) return BadRequest();
            return RedirectToAction(nameof(List));
        }

        [Route("/products/{productId:int}/enable")]
        [HttpGet]
        public async Task<IActionResult> Enable([FromRoute] int productId)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsync($"/api/admin/products/{productId}/enable", null);
            if (!response.IsSuccessStatusCode) return BadRequest();
            return RedirectToAction(nameof(List));
        }

        [Route("/products/{productId:int}/delete")]
        [HttpGet]
        public IActionResult Delete([FromRoute] int productId)
        {
            return View();
        }
    }
}