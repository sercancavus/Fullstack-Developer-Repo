using App.Data.Entities;
using App.Data.Infrastructure;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.ViewComponents
{
    public class FromTheBlogViewComponent(DataRepository<BlogEntity> repo) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = await repo.GetAll()
                    .Where(e => e.Enabled)
                    .OrderByDescending(e => e.CreatedAt)
                    .Take(3)
                    .Select(e => new BlogSummaryViewModel
                    {
                        Id = e.Id,
                        Title = e.Title,
                        SummaryContent = e.Content.Substring(0, 100),
                        ImageUrl = e.ImageUrl,
                        CommentCount = e.Comments.Count,
                        CreatedAt = e.CreatedAt
                    })
                    .ToListAsync();

            return View(viewModel);
        }
    }
}