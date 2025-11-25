using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace App.Eticaret.ViewComponents
{
    public class ReviewProductsViewComponent(IHttpClientFactory httpClientFactory) : ViewComponent
    {
        public class ProductDto { public int Id { get; set; } public string Name { get; set; } = string.Empty; public decimal Price { get; set; } public string CategoryName { get; set; } = string.Empty; public byte? DiscountPercentage { get; set; } public string? ImageUrl { get; set; } public int ReviewCount { get; set; } }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = httpClientFactory.CreateClient("DataApi");
                var data = await client.GetFromJsonAsync<List<ProductDto>>("/api/product/list");
                var vm = new OwlCarouselViewModel
                {
                    Title = "Review Products",
                    Items = (data ?? new()).OrderByDescending(p => p.ReviewCount).Take(6).Select(p => new ProductListingViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        CategoryName = p.CategoryName,
                        DiscountPercentage = p.DiscountPercentage,
                        ImageUrl = p.ImageUrl
                    }).ToList()
                };
                return View(vm);
            }
            catch
            {
                return View(new OwlCarouselViewModel { Title = "Review Products", Items = new List<ProductListingViewModel>() });
            }
        }
    }
}