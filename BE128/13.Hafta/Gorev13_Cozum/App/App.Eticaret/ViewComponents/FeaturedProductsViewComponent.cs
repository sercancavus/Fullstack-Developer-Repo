using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.ViewComponents
{
    public class FeaturedProductsViewComponent(ApplicationDbContext context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await context.Products
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
