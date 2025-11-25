using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Eticaret.ViewComponents
{
    public class CategoryListViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
    {
        public class CategoryDto { public int Id { get; set; } public string Name { get; set; } = string.Empty; public string Color { get; set; } = string.Empty; public string IconCssClass { get; set; } = string.Empty; }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = httpClientFactory.CreateClient("DataApi");
                var data = await client.GetFromJsonAsync<List<CategoryDto>>("/api/category");
                var categories = (data ?? new()).Select(c => new CategoryListViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Color = c.Color,
                    IconCssClass = c.IconCssClass
                }).ToList();
                return View(categories);
            }
            catch
            {
                return View(new List<CategoryListViewModel>());
            }
        }
    }
}