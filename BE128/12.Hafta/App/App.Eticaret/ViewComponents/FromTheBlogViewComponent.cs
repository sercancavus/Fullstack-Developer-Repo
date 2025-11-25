using App.Data.Contexts;
using App.Eticaret.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Eticaret.ViewComponents
{
    public class FromTheBlogViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _dbContext;

        public FromTheBlogViewComponent(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = await _dbContext.Blogs
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
