using App.Data.Entities;
using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.ViewComponents
{
    public class FeaturedProductsViewComponent(DataRepository<ProductEntity> repo) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await repo.GetAll()
                .Where(p => p.Enabled)
                .Select(p => new FeaturedProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    DiscountPercentage = p.Discount == null ? null : p.Discount.DiscountRate,
                    ImageUrl = p.Images.Count != 0 ? p.Images.First().Url : null
                })
                .ToListAsync();

            return View(products);
        }
    }
}