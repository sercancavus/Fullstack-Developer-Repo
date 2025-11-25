using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.ViewComponents
{
    public class BlogCategoriesSidebarViewComponent(ApplicationDbContext context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await context.BlogCategories
                .Select(c => new BlogCategorySidebarViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ArticleCount = c.BlogRelations.Count
                })
                .ToListAsync();

            return View(model);
        }
    }
}