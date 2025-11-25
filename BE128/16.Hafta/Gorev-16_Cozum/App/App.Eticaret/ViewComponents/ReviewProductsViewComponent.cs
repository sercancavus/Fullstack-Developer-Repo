using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace App.Eticaret.ViewComponents
{
    public class ReviewProductsViewComponent(IHttpClientFactory clientFactory) : ViewComponent
    {
        private HttpClient Client => clientFactory.CreateClient("Api.Data");

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await Client.GetAsync("products/review?take=6");

            if (!response.IsSuccessStatusCode)
            {
                return Content("Veri alınamadı.");
            }

            var model = await response.Content.ReadFromJsonAsync<OwlCarouselViewModel>();

            return View(model);

            //var viewModel = new OwlCarouselViewModel
            //{
            //    Title = "Review Products",
            //    Items = await repo.GetAll()
            //        .Where(p => p.Enabled)
            //        .OrderByDescending(p => p.Comments.Count)
            //        .Take(6)
            //        .Select(p => new ProductListingViewModel
            //        {
            //            Id = p.Id,
            //            Name = p.Name,
            //            Price = p.Price,
            //            CategoryName = p.Category.Name,
            //            DiscountPercentage = p.Discount == null ? null : p.Discount.DiscountRate,
            //            ImageUrl = p.Images.Count != 0 ? p.Images.First().Url : null
            //        })
            //        .ToListAsync()
            //};

            //return View(viewModel);
        }
    }
}