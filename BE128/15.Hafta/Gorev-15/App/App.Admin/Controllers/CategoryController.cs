using App.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Admin.Controllers
{
    [Route("/categories")]
    [Authorize(Roles = "admin")]
    public class CategoryController(IHttpClientFactory httpClientFactory) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var categories = await client.GetFromJsonAsync<List<CategoryListViewModel>>("/api/category");
            return View(categories ?? new List<CategoryListViewModel>());
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SaveCategoryViewModel newCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(newCategoryModel);
            }

            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PostAsJsonAsync("/api/category", new
            {
                name = newCategoryModel.Name,
                color = newCategoryModel.Color,
                iconCssClass = newCategoryModel.IconCssClass ?? string.Empty
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kategori oluşturulamadı.");
                return View(newCategoryModel);
            }

            ViewBag.SuccessMessage = "Kategori başarıyla oluşturuldu.";
            ModelState.Clear();

            return View();
        }

        [Route("{categoryId:int}/edit")]
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int categoryId)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var category = await client.GetFromJsonAsync<SaveCategoryViewModel>($"/api/category/{categoryId}");
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [Route("{categoryId:int}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int categoryId, [FromForm] SaveCategoryViewModel editCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editCategoryModel);
            }

            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.PutAsJsonAsync($"/api/category/{categoryId}", new
            {
                name = editCategoryModel.Name,
                color = editCategoryModel.Color,
                iconCssClass = editCategoryModel.IconCssClass ?? string.Empty
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kategori güncellenemedi.");
                return View(editCategoryModel);
            }

            ViewBag.SuccessMessage = "Kategori başarıyla güncellendi.";
            ModelState.Clear();

            return View(editCategoryModel);
        }

        [Route("{categoryId:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int categoryId)
        {
            var client = httpClientFactory.CreateClient("DataApi");
            var response = await client.DeleteAsync($"/api/category/{categoryId}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(List));
        }
    }
}