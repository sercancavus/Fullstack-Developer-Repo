using App.Data.Entities;
using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.ViewComponents
{
    public class CategoryListViewComponent(DataRepository<CategoryEntity> repo) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await repo.GetAll()
                .Select(c => new CategoryListViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Color = c.Color,
                    IconCssClass = c.IconCssClass
                })
                .ToListAsync();
            return View(categories);
        }
    }
}