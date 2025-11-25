using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.Controllers
{
    [Route("blog")]
    public class BlogController(IHttpClientFactory clientFactory) : BaseController
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // TODO: seed data in api project
            var response = await Client.GetAsync("blog");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }
            var content = await response.Content.ReadFromJsonAsync<List<BlogSummaryViewModel>>();
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await Client.GetAsync($"blog/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var content = await response.Content.ReadFromJsonAsync<BlogDetailViewModel>();
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }
    }
}