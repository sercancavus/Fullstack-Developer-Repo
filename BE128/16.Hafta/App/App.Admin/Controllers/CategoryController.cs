using App.Admin.Models.ViewModels;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Admin.Controllers
{
    [Route("/categories")]
    [Authorize(Roles = "admin")]
    public class CategoryController(IHttpClientFactory clientFactory) : Controller
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await Client.GetAsync("api/category");

            List<CategoryListViewModel> model = [];

            if (!response.IsSuccessStatusCode)
            {
                return View(model);
            }

            model = (await response.Content.ReadFromJsonAsync<List<CategoryListViewModel>>())
                ?? [];

            return View(model);
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

            var categoryEntity = new CategoryEntity
            {
                Name = newCategoryModel.Name,
                Color = newCategoryModel.Color,
                IconCssClass = string.Empty,
            };

            var response = await Client.PostAsJsonAsync("api/category", categoryEntity);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kategori oluşturulurken bir hata oluştu.");
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

            var response = await Client.GetAsync($"api/category/{categoryId}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var category = await response.Content.ReadFromJsonAsync<CategoryEntity>();

            if (category is null)
            {
                return NotFound();
            }

            var editCategoryModel = new SaveCategoryViewModel
            {
                Name = category.Name,
                Color = category.Color,
                IconCssClass = category.IconCssClass
            };

            return View(editCategoryModel);
        }

        [Route("{categoryId:int}/edit")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int categoryId, [FromForm] SaveCategoryViewModel editCategoryModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editCategoryModel);
            }

            var response = await Client.GetAsync($"api/category/{categoryId}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var category = await response.Content.ReadFromJsonAsync<CategoryEntity>();
            if (category is null)
            {
                return NotFound();
            }

            category.Name = editCategoryModel.Name;
            category.Color = editCategoryModel.Color;
            category.IconCssClass = editCategoryModel.IconCssClass ?? string.Empty;

            var updateResponse = await Client.PutAsJsonAsync($"api/category/{categoryId}", category);

            if (!updateResponse.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Kategori güncellenirken bir hata oluştu.");
                return View(editCategoryModel);
            }

            ViewBag.SuccessMessage = "Kategori başarıyla güncellendi.";
            ModelState.Clear();

            return View();
        }

        [Route("{categoryId:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int categoryId)
        {
            var response = await Client.DeleteAsync($"api/category/{categoryId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(List));
        }
    }
}