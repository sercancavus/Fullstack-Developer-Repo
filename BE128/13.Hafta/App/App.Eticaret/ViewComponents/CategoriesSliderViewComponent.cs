using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.ViewComponents
{
    public class CategoriesSliderViewComponent(ApplicationDbContext context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await context.Products
                .GroupBy(p => p.CategoryId)
                .Select(g => new CategorySliderViewModel
                {
                    Id = g.First().Category.Id,
                    Name = g.First().Category.Name,
                    Color = g.First().Category.Color,
                    IconCssClass = g.First().Category.IconCssClass,
                    ImageUrl = g.First().Images.Count != 0 ? g.First().Images.First().Url : null
                })
                .ToListAsync();

            return View(categories);
        }
    }
}