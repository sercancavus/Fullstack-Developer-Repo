using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.ViewComponents
{
    public class FeaturedProductsViewComponent(IHttpClientFactory clientFactory) : ViewComponent
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await Client.GetAsync("products/featured");

            if (!response.IsSuccessStatusCode)
            {
                return Content("Veri alınamadı.");
            }

            var model = await response.Content.ReadFromJsonAsync<List<FeaturedProductViewModel>>();

            return View(model);

            //var products = await repo.GetAll()
            //    .Where(p => p.Enabled)
            //    .Select(p => new FeaturedProductViewModel
            //    {
            //        Id = p.Id,
            //        Name = p.Name,
            //        Price = p.Price,
            //        CategoryName = p.Category.Name,
            //        DiscountPercentage = p.Discount == null ? null : p.Discount.DiscountRate,
            //        ImageUrl = p.Images.Count != 0 ? p.Images.First().Url : null
            //    })
            //    .ToListAsync();

            //return View(products);
        }
    }
}