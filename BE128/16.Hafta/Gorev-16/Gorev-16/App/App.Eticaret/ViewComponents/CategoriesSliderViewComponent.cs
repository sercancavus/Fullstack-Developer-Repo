using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Eticaret.ViewComponents
{
    public class CategoriesSliderViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
    {
        public class CategoryDto { public int Id { get; set; } public string Name { get; set; } = string.Empty; public string Color { get; set; } = string.Empty; public string IconCssClass { get; set; } = string.Empty; public string? ImageUrl { get; set; } }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = httpClientFactory.CreateClient("DataApi");
                var categories = await client.GetFromJsonAsync<List<CategoryDto>>("/api/category");
                var vm = (categories ?? new()).Select(c => new CategorySliderViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Color = c.Color,
                    IconCssClass = c.IconCssClass,
                    ImageUrl = c.ImageUrl
                }).ToList();
                return View(vm);
            }
            catch
            {
                return View(new List<CategorySliderViewModel>());
            }
        }
    }
}