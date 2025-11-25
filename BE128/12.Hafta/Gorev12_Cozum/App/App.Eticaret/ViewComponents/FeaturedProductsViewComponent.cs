using App.Data.Contexts;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.ViewComponents
{
    public class FeaturedProductsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _dbContext;

        public FeaturedProductsViewComponent(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _dbContext.Products
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
